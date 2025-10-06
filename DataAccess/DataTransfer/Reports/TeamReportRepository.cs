using DataAccess.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataTransfer.Reports
{
    public class TeamReportRepository : BaseReport, ITeamReportRepository
    {
        public TeamReportRepository(IDbConnection db, ILogger<TeamReportRepository> log) : base(db, log)
        {
        }

        public IEnumerable<TeamDto> GetTeamsWithoutVacations2019()
        {
            var sql = @"
            SELECT t.Id, t.Name FROM Team t
            WHERE NOT EXISTS (
                SELECT 1 FROM Employee e
                JOIN Vacation v ON e.Id = v.EmployeeId
                WHERE e.TeamId = t.Id
                  AND v.DateSince >= DATE('2019-01-01')
                  AND v.DateSince <= DATE('2019-12-31')
            );";

            var teams = ExecuteQuery<TeamDto>(sql);

            return teams;
        }
    }

}
