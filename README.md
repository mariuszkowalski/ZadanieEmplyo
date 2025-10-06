
### Wstęp

Nie wiedziałem, czy napisać ten dokument w języku polskim, czy w angielskim.
Uznałem, że jeśli instrukcje do zadania był w języku polskim, to wyjaśnienie też powinno być w języku polskim.

Aczkolwiek komentarze w kodzie, jeśli się znajdują, są w języku angielskim, tak samo jak nazwy klas i metod.

Opis struktury całego projektu znajduje się poniżej (za sekcją `Odpowiedzi`), wraz z wyjaśnieniami dotyczącymi zastosowanych rozwiązań oraz bibliotek.

### Odpowiedzi

Znajdują się również w kodzie, dodałem je również tutaj, aby były łatwiejsze do przejrzenia.

#### Zadanie 1.

Lokalizacja plików w projekcie:
```
Services
   |-- Structure
   |   |-- EmployeeStructure.cs
   |   |-- EmployeeStructureService.cs
```

EmployeeStructure.cs
```csharp
public class EmployeeStructure
{
    public int EmployeeId { get; set; }
    public int SuperiorId { get; set; }
    public int Rank { get; set; }
}
```

EmployeeStructureService.cs
```csharp
public class EmployeeStructureService
{
    private readonly List<EmployeeStructure> _structure;

    public EmployeeStructureService()
    {
        _structure = new List<EmployeeStructure>();
    }

    public List<EmployeeStructure> FillEmployeesStructure(List<EmployeeDto> employees)
    {
        _structure.Clear();

        foreach (var emp in employees)
        {
            AddRelationsRecursive(emp, emp.SuperiorId, 1, employees);
        }

        return _structure;
    }

    private void AddRelationsRecursive(EmployeeDto employee, int? superiorId, int rank, List<EmployeeDto> allEmployees)
    {
        if (superiorId == null)
        {
            return;
        }

        _structure.Add(new EmployeeStructure
        {
            EmployeeId = employee.Id,
            SuperiorId = superiorId.Value,
            Rank = rank
        });

        var superior = allEmployees.FirstOrDefault(employee => employee.Id == superiorId.Value);
        
        if (superior != null && superior.SuperiorId.HasValue)
        {
            AddRelationsRecursive(employee, superior.SuperiorId, rank + 1, allEmployees);
        }
    }

    public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
    {
        return _structure.FirstOrDefault(r => r.EmployeeId == employeeId && r.SuperiorId == superiorId)?.Rank;
    }
}
```

Założone podejście jest minimalistyczne. Można było użyć `Composite Design Pattern`, ale komplikowało to kod bardziej niż to było konieczne.

#### Zadanie 2.

Lokalizacja plików w projekcie, a raczej gdzie znajduje się kod sql:
- `TeamReportRepository`
- `VacationReportRepository`

```
DataAccess
   |-- DataTransfer
   |   |-- Reports
   |   |   |-- BaseReport.cs
   |   |   |-- Interfaces
   |   |   |   |-- ITeamReportRepository.cs
   |   |   |   |-- IVacationReportRepository.cs
   |   |   |-- TeamReportRepository.cs
   |   |   |-- VacationReportRepository.cs
```

Użyłem bazy danych `Sqlite`.

Problem "A"
```sql
SELECT DISTINCT e.Id, e.Name FROM Employee e
JOIN Team t ON e.TeamId = t.Id
JOIN Vacation v ON e.Id = v.EmployeeId
WHERE t.Name = '.NET'
    AND v.DateSince >= DATE('2019-01-01')
    AND v.DateSince <= DATE('2019-12-31');
```

Problem "B"
Założyłem dziań pracy jako 8h.
Zdaję sobie sprawę, że można pracować w trybie 10h jak i to, że np. osoba niepełnosprawna pracuje 7h. Rozumiem jednak, że celem tego zadania jest sprawdzenie znajomości `sql`, a nie prawa pracy.

```sql
SELECT e.Id, e.Name, (SUM(v.NumberOfHours)/8) AS UsedDays FROM Employee e
LEFT JOIN Vacation v ON e.Id = v.EmployeeId
WHERE v.DateSince >= DATE(strftime('%Y','now') || '-01-01')
    AND v.DateSince <= DATE(strftime('%Y','now') || '-12-31')
    AND v.DateUntil < DATE('now')
GROUP BY e.Id, e.Name;
```

Problem "C"
```sql
SELECT t.Id, t.Name FROM Team t
WHERE NOT EXISTS (
    SELECT 1 FROM Employee e
    JOIN Vacation v ON e.Id = v.EmployeeId
    WHERE e.TeamId = t.Id
        AND v.DateSince >= DATE('2019-01-01')
        AND v.DateSince <= DATE('2019-12-31')
);
```

#### Zadanie 3 oraz 4.

Lokalizacja plików w projekcie:
```
Services
   |-- VacationCalculation
   |   |-- IVacationCalculationService.cs
   |   |-- VacationCalculationService.cs
```

VacationCalculationService.cs
```csharp
public class VacationCalculationService : IVacationCalculationService
{
    private readonly ILogger<VacationCalculationService> _log;
    public VacationCalculationService(ILogger<VacationCalculationService> log)
    {
        _log = log;
    }

    public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
    {
        try
        {
            employee.CheckIfNotNull(nameof(employee), _log);
            vacationPackage.CheckIfNotNull(nameof(vacationPackage), _log);
            vacations.CheckIfNotNull(nameof(vacations), _log);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Validation failed.");
            
            throw;
        }

        var year = DateTime.Now.Year;

        var vacationsThisYear = vacations.Where(v => (v.DateSince.Year == year || v.DateUntil.Year == year) && v.EmployeeId == employee.Id).ToList();

        var totalHoursUsed = 0;

        // I assume that the day of vacation corresponds to 8h.
        // I don't have enough information so I assume worst case scenario that sometimes
        // the NumberOfHours is 0 due to errors, 0 != null so it will pass.
        foreach (var vacation in vacationsThisYear)
        {
            if (vacation.NumberOfHours > 0)
            {
                totalHoursUsed += vacation.NumberOfHours;
            }
            else
            {
                var days = (vacation.DateUntil.Date - vacation.DateSince.Date).Days + 1;
                totalHoursUsed += days * 8;
            }
        }

        var usedDays = totalHoursUsed / 8;
        var remainingDays = vacationPackage.GrantedDays - usedDays;

        return remainingDays;
    }

    public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
    {
        var remainingDays = CountFreeDaysForEmployee(employee, vacations, vacationPackage);
        
        var canRrequestVacation = remainingDays > 0;

        return canRrequestVacation;
    }

}
```

Na wejściu do metody `CountFreeDaysForEmployee` sprawdzam, czy przekazane argumenty nie są `null`.
Sprawdzenie jest dokonywane z pomocą `extension method`. Error jest logowany dwukrotnie, ale logowanie np. wewnątrz `CheckIfNotNull` można dodatkowo oznaczyć, co może być przydatne do analizy logów i błędów w aplikacji.
Ewentualnie, w przypadku błędu można by zwracać wartość `-1`, ponieważ nie można mieć minusowej wartości wolnych dni urlopu. Jednak w takim wypadku w `IfEmployeeCanRequestVacation` nadal pozostaje jedynie możliwość zwrócenia wartości `bool`. Do obsługi błędów w tych obydwu przypadkach można by użyć np. `FluentResult`, lub stworzyć ręcznie klasę, która zawierała by informacje o sukcesie operacji, wartość oraz kod błędu i była zwracana zamiast `int` lub `bool`. Aczkolwiek, takie rozwiązanie byłoby niezgodne z instrukcją z zadania. 

Nie sprawdzam już nulli na wejściu do `IfEmployeeCanRequestVacation` gdyż zaraz wywołuje ona `CountFreeDaysForEmployee`, która nulle sprawdza. 


#### Zadanie 5.

Lokalizacja plików w projekcie:
```
Services.Tests
   |-- VacationCalculationServiceTests.cs
```

VacationCalculationServiceTests.cs
```csharp
public class VacationCalculationServiceTests
{
    private VacationCalculationService _service;
    private Mock<ILogger<VacationCalculationService>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<VacationCalculationService>>();
        _service = new VacationCalculationService(_loggerMock.Object);
    }

    [Test]
    public void employee_can_request_vacation()
    {
        var employee = new Employee { Id = 1, Name = "Mariusz Kowalski", TeamId = 1 };
        var vacationPackage = new VacationPackage { Id = 1, Name = "Standard", GrantedDays = 26, Year = 2025 };

        var vacations = new List<Vacation>
        {
            new() {
                Id = 1,
                EmployeeId = 1,
                DateSince = new DateTime(DateTime.Now.Year, 1, 1),
                DateUntil = new DateTime(DateTime.Now.Year, 1, 5),
                NumberOfHours = 40
            }
        };

        var result = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public void employee_cant_request_vacation()
    {
        var employee = new Employee { Id = 2, Name = "Jan Nowak", TeamId = 1 };
        var vacationPackage = new VacationPackage { Id = 1, Name="Micro", GrantedDays = 10, Year=2025 };

        var vacations = new List<Vacation>
        {
            new() {
                Id = 1,
                EmployeeId = 2,
                DateSince = new DateTime(DateTime.Now.Year, 1, 1),
                DateUntil = new DateTime(DateTime.Now.Year, 1, 10),
                NumberOfHours = 80
            }
        };

        var result = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

        Assert.That(result, Is.False);
    }
}
```

#### Zadanie 6.

1. Zamiast pobierać dane osobno dla danego obiektu `Dto` można połączyć zapytanie do kilku tabel za pomocą `join`.
2. Filtrować dane już na poziomie bazy. To znaczy odpowiednie użycie `WHERE`
3. Operacje takie jak `SUM` oraz inne agregacje danych też lepiej przeprowadzać od razu z poziomu sql. Silniki baz są do tych celów optymalizowane.
4. Cache'oweanie pobranych danych, aby nie odpytywać bazy wielokrotnie o te same dane.
5. Zastosowanie procedur (brak w Sqlite).
6. Stworzenie widoku indeksowanego (brak w Sqlite). 


### Opis projektu

Zastosowałem biblioteki takie jak:
- Serilog - logowanie
- Dapper  - obsługa połączeń z bazą danych
- Microsoft.Extensions.DependencyInjection - aby mieć DI container.
- Microsoft.Extensions.Caching.Memory - do cache'owania zapytń.
- NUnit - testy jednostkow.
- Moq - do mockowania w teście jednostkowym.
- Spectre.console - do menu.

Pierwsze pytanie, które może się od razu nasunąć: "Dlaczego ten projekt jest tak duży?"
W mailu mowa była o projekcie (w jednym punkcie) oraz o strukturze kodu, a w pliku pdf dostałem listę pytań, na które można odpowiedzieć, używając prostej konsolowej aplikacji. Raz przejechałem się na tym, że zrobiłem "*za mało*", także stwierdziłem, że stworzę cały projekt z zastosowaniem obowiązujących zasad.

Cała solucja podzielona jest na poniższe projekty, separujące poszczególne warstwy.

#### DataAccess

Zawiera obsługę połączeń do bazy danych.
Każda Tabela ma swój osobny Data Access Object.
W celu optymalizacji zapytań każde dao posiada Memory Cache, aby ograniczyć ilość zapytań do bazy.
Każdy Cache ma inny TTL. Team oraz VacationPackage najdłuższy - najrzadziej się zmieniają. Vacation ma najkrótszy. Teoretycznie ze wszystkich danych, te dane mogą ulegać najczęstszej modyfikacji, tym bardziej przed gorącymi okresami urlopowymi.

Dao zawierają tylko proste metody, odpytywanie pojedynczej tabeli, tworzenie, modyfikowanie oraz usuwanie danych z tej tabeli.
Zapytania o ilość pozostałych dni wolnych, o to czy pracownik, może wziąć urlop oraz o zespół bez dni urlopowych zostały umieszczone w repozytoriach raportów, aby nie zaśmiecać dao. Są różne podejścia do tego problemu, wybrałem akurat takie.

Aby lepiej odseparować "Widok", za który odpowiada tutaj konsolowa aplikacja. Employee, Vacation etc. dostały swoje dedykowane `Dto`. Są one małe, więc wybrałem opcję ręcznego mapowania.

#### Services.

Każde Dao z projektu DataAccess ma swój własny serwis. Raporty dostały również swoje własne serwisy. Klasy z zadań również mają swoje serwisy.

Znajdują się tutaj również dwie klasy rozszerzeń, w katalogu `Helpers`. Jedna dla `ILogger` pozwalająca na logowanie jednocześnie w logach oraz w konsoli aplikacji. Druga do sprawdzania czy obiekt jest nullem i logująca jeśli jest, a jeśli nie to zwracająca wartość. W Servisie znajdują się również Mappery utworzone ręcznie (też klasy rozszerzeń).

Generalnie słysząc "raport" widzi się oczami wyobraźni sql na 100 linii kodu, który wykonuje się kilkadziesiąt sekund. Kiedy pod koniec dnia działy produkcyjne zaczynają generować raporty serwer zaczyna płonąć. W związku z tym, raporty, które utworzyłem też dostały swoje chache, jednakże na poziomie serwisu. Aby ich cache pozostawał aktualny, w razie zmian na Tabelach, to powstał CacheInvalidationHandler oraz EventBuss. Jeśli coś zostanie do tabeli dodane, zmodyfikowane, albo usunięte cache raportów na poziomie serwisów jest unieważniany.

#### Recruitment.

Konsolowa aplikacja, używająca Spectre.console. Umożliwia to dodanie kolorów do oraz menu pozwalającego na dopalenie kodu z zadań (chciałem uniknąć WPF). Startup, klasycznie odpowiada za utworzenie service provider, konfigurację logów, IDbConnection oraz dodawanie serwisów. W App.cs znajduje się menu.
