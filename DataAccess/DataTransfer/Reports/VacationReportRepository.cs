using DataAccess.DataTransfer.Reports;
using DataAccess.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;


public class VacationReportRepository : BaseReport, IVacationReportRepository
{
    public VacationReportRepository(IDbConnection db, ILogger<VacationReportRepository> log) : base(db, log)
    {
    }

    // Case 1
    public IEnumerable<EmployeeDto> GetEmployeesInDotNetTeamWithVacations2019()
    {
        var sql = @"
            SELECT DISTINCT e.Id, e.Name FROM Employee e
            JOIN Team t ON e.TeamId = t.Id
            JOIN Vacation v ON e.Id = v.EmployeeId
            WHERE t.Name = '.NET'
              AND v.DateSince >= DATE('2019-01-01')
              AND v.DateSince <= DATE('2019-12-31');";


        var employees = ExecuteQuery<EmployeeDto>(sql);

        return employees;
    }

    // Case 2
    public IEnumerable<EmployeeVacationUsageDto> GetVacationUsageThisYear()
    {
        var sql = @"
            SELECT e.Id, e.Name, (SUM(v.NumberOfHours)/8) AS UsedDays FROM Employee e
            LEFT JOIN Vacation v ON e.Id = v.EmployeeId
            WHERE v.DateSince >= DATE(strftime('%Y','now') || '-01-01')
              AND v.DateSince <= DATE(strftime('%Y','now') || '-12-31')
              AND v.DateUntil < DATE('now')
            GROUP BY e.Id, e.Name;";

        var evus = ExecuteQuery<EmployeeVacationUsageDto>(sql);

        return evus;
    }


}
