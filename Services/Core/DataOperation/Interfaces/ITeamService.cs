using DataAccess.Models;

namespace Services.Core.DataOperation.Interfaces
{
    public interface ITeamService
    {
        void InitializeTable();
        bool HasData();
        bool Add(TeamDto dto);
        bool Delete(int id);
        IEnumerable<TeamDto> GetAll();
        TeamDto? GetById(int id);
        bool Update(TeamDto dto);
    }
}