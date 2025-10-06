using DataAccess.Models;

namespace Services.Core.DataOperation.Interfaces
{
    public interface IVacationReportService
    {
        IEnumerable<EmployeeDto> GetEmployeesInDotNetTeamWithVacations2019();
        IEnumerable<EmployeeVacationUsageDto> GetVacationUsageThisYear();
        void InvalidateVacationUsageCache();
    }
}