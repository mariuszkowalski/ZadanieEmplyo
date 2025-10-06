using Services.Core.DataOperation.Models;

namespace Services.VacationCalculation
{
    public interface IVacationCalculationService
    {
        int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage);
        bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage);
    }
}