using DataAccess.Models;

namespace Services.Core.DataOperation
{
    public interface ITeamReportService
    {
        IEnumerable<TeamDto> GetTeamsWithoutVacations2019();
        void InvalidateVacationUsageCache();
    }
}