using DataAccess.Models;

namespace DataAccess.DataTransfer.Reports
{
    public interface ITeamReportRepository
    {
        IEnumerable<TeamDto> GetTeamsWithoutVacations2019();
    }
}