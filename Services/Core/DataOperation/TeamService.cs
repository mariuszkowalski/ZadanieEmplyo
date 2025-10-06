using DataAccess.DataTransfer.Interfaces;
using DataAccess.Models;
using Services.Core.DataOperation.Interfaces;
using Services.Core.DataOperation.InvalidationHandlers;
using Services.Core.DataOperation.InvalidationHandlers.Interfaces;
using System.Security;

namespace Services.Core.DataOperation
{
    public class TeamService : ITeamService
    {
        private readonly ITeamDao _dao;
        private readonly IEventBus _eventBus;

        public TeamService(ITeamDao dao, IEventBus bus)
        {
            _dao = dao;
            _eventBus = bus;
        }

        public void InitializeTable() => _dao.InitializeTable();
        public bool HasData() => _dao.HasData();
        public IEnumerable<TeamDto> GetAll() => _dao.GetAllTeams();

        public TeamDto? GetById(int id) => _dao.GetTeamById(id);

        public bool Add(TeamDto dto)
        { 
            var success = _dao.Insert(dto);

            if (success)
            {
                _eventBus.Publish(new TeamAdded());
            }

            return success;
        }

        public bool Update(TeamDto dto)
        {
            var success = _dao.Update(dto);
            
            if (success)
            {
                _eventBus.Publish(new TeamAdded());
            }
            
            return success;
        }

        public bool Delete(int id)
        {
            var success = _dao.Delete(id);
            
            if (success)
            {
                _eventBus.Publish(new TeamAdded());
            }
            
            return success;
        }
    }


}
