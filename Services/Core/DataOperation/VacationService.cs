using DataAccess.DataTransfer.Interfaces;
using DataAccess.Models;
using Services.Core.DataOperation.Interfaces;
using Services.Core.DataOperation.InvalidationHandlers;
using Services.Core.DataOperation.InvalidationHandlers.Interfaces;


namespace Services.Core.DataOperation
{
    public class VacationService : IVacationService
    {
        private readonly IVacationDao _dao;
        private readonly IEventBus _eventBus;

        public VacationService(IVacationDao dao, IEventBus eventBus)
        {
            _dao = dao;
            _eventBus = eventBus;
        }

        public void InitializeTable() => _dao.InitializeTable();
        public bool HasData() => _dao.HasData();
        public IEnumerable<VacationDto> GetAll() => _dao.GetAllVacations();

        public VacationDto? GetById(int id) => _dao.GetVacationById(id);

        public bool Add(VacationDto dto)
        {
            var success = _dao.Insert(dto);
            if (success)
            {
                _eventBus.Publish(new VacationAdded());
            }

            return success;
        }

        public bool Update(VacationDto dto)
        {
            var success = _dao.Update(dto);
            if (success)
            {
                _eventBus.Publish(new VacationUpdated());
            }
            
            return success; 
        }

        public bool Delete(int id)
        {
            var success = _dao.Delete(id);
            if (success)
            {
                _eventBus.Publish(new VacationDeleted());
            }

            return success;
        }
    }


}
