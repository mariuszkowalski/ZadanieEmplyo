using DataAccess.Models;



namespace DataAccess.DataTransfer.Interfaces
{
    public interface ITeamDao
    {
        void InitializeTable();
        bool HasData();
        bool Delete(int id);
        IEnumerable<TeamDto> GetAllTeams();
        TeamDto? GetTeamById(int id);
        bool Insert(TeamDto dto);
        bool Update(TeamDto dto);
    }
}
