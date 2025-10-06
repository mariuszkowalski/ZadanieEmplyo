using DataAccess.Models;

public interface IVacationReportRepository
{
    IEnumerable<EmployeeDto> GetEmployeesInDotNetTeamWithVacations2019();
    IEnumerable<EmployeeVacationUsageDto> GetVacationUsageThisYear();
}