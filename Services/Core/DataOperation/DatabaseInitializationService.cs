using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Helpers;
using Services.Core.DataOperation.Interfaces;
using Services.Core.DataOperation.Models;
using Services.Core.DataOperation.Mappers;

namespace Services.Core.DataOperation
{

    public class DatabaseInitializerService
    {
        private readonly ILogger<DatabaseInitializerService> _log;
        private readonly IEmployeeService _employeeService;
        private readonly ITeamService _teamService;
        private readonly IVacationPackageService _vacationPackageService;
        private readonly IVacationService _vacationService;

        public DatabaseInitializerService(ILogger<DatabaseInitializerService> log, IEmployeeService employeeService, ITeamService teamService,
            IVacationPackageService packageService, IVacationService vacationPackageService)
        {
            _log = log;
            _employeeService = employeeService;
            _teamService = teamService;
            _vacationPackageService = packageService;
            _vacationService = vacationPackageService;
        }

        public void InitializeDatabase(bool seedData = false)
        {
            _log.InfoWithConsole("Initializing database...");
            
            // Table initialization.
            _employeeService.InitializeTable();
            _teamService.InitializeTable();
            _vacationPackageService.InitializeTable();
            _vacationService.InitializeTable();

            _log.InfoWithConsole("All tables are initialized.");

            if (seedData)
            {
                SeedData();
            }
        }

        private void SeedData()
        {
            _log.InfoWithConsole("Seeding initial data...");

            if (!_teamService.HasData())
            {
                var team1 = new Team { Name = ".NET" };
                var team2 = new Team { Name = "HR" };
                var team3 = new Team { Name = "DevOps" };
                _teamService.Add(team1.ToDto());
                _teamService.Add(team2.ToDto());
                _teamService.Add(team3.ToDto());
                _log.InfoWithConsole("Team table seeded.");
            }
            else
            {
                _log.WarnWithConsole("Team table already has been populated with seed data.");
            }


            // I am fully aware that there are other 'packages' for example for people with disabilitess.
            // However goal of this exercise is to check my software development skills, not my labor law knowledge.
            // In other words I will only stick with these two - commonly known packages.
            if (!_vacationPackageService.HasData())
            {
                var pkg1 = new VacationPackage { Name = "Standard", GrantedDays = 21, Year = 2019 };
                var pkg2 = new VacationPackage { Name = "Big", GrantedDays = 26, Year = 2019 };
                _vacationPackageService.Add(pkg1.ToDto());
                _vacationPackageService.Add(pkg2.ToDto());

                _log.InfoWithConsole("Vacation package table seeded.");
            }
            else
            {
                _log.WarnWithConsole("VacationPackage table already has been populated with seed data.");
            }

            if (!_employeeService.HasData())
            {
                var employee1 = new Employee { Name = "Mariusz Kowalski", TeamId = 1, VacationPackage = 2 };
                var employee2 = new Employee { Name = "Tomasz Kwarc", TeamId = 1, VacationPackage = 2 };
                var employee3 = new Employee { Name = "Mariano Italiano", TeamId = 2, VacationPackage = 2 };
                var employee4 = new Employee { Name = "Aleksandr Kerensky", TeamId = 3, VacationPackage = 2 };
                _employeeService.Add(employee1.ToDto());
                _employeeService.Add(employee2.ToDto());
                _employeeService.Add(employee3.ToDto());
                _employeeService.Add(employee4.ToDto());

                _log.InfoWithConsole("Employee table seeded.");
            }
            else
            {
                _log.WarnWithConsole("Employee table already has been populated with seed data.");
            }

            if (!_vacationService.HasData())
            {
                var vacation1 = new Vacation
                {
                    EmployeeId = 1,
                    DateSince = DateTime.ParseExact("2019-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    DateUntil = DateTime.ParseExact("2019-07-05", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    NumberOfHours = 40,
                    IsPartialVacation = 0
                };
                var vacation2 = new Vacation
                {
                    EmployeeId = 1,
                    DateSince = DateTime.ParseExact("2019-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    DateUntil = DateTime.ParseExact("2019-08-03", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    NumberOfHours = 24,
                    IsPartialVacation = 0
                };
                var vacation3 = new Vacation
                {
                    EmployeeId = 2,
                    DateSince = DateTime.ParseExact("2019-06-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    DateUntil = DateTime.ParseExact("2019-06-05", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    NumberOfHours = 40,
                    IsPartialVacation = 0
                };
                var vacation4 = new Vacation
                {
                    EmployeeId = 3,
                    DateSince = DateTime.ParseExact("2019-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    DateUntil = DateTime.ParseExact("2019-07-14", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    NumberOfHours = 80,
                    IsPartialVacation = 0
                };
                var vacation5 = new Vacation
                {
                    EmployeeId = 1,
                    DateSince = DateTime.ParseExact("2025-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    DateUntil = DateTime.ParseExact("2025-07-14", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    NumberOfHours = 80,
                    IsPartialVacation = 0
                };
                _vacationService.Add(vacation1.ToDto());
                _vacationService.Add(vacation2.ToDto());
                _vacationService.Add(vacation3.ToDto());
                _vacationService.Add(vacation4.ToDto());
                _vacationService.Add(vacation5.ToDto());

                _log.InfoWithConsole("Vacation table seeded.");
            }
            else
            {
                _log.WarnWithConsole("Vacation table already has been populated with seed data.");
            }


            _log.InfoWithConsole("Seed data inserted successfully.");
        }
    }
}
