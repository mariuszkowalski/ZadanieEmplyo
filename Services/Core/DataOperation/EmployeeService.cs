using DataAccess.DataTransfer;
using DataAccess.DataTransfer.Interfaces;
using DataAccess.Models;
using Services.Core.DataOperation.Interfaces;
using Services.Core.DataOperation.InvalidationHandlers;
using Services.Core.DataOperation.InvalidationHandlers.Interfaces;

namespace Services.Core.DataOperation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDao _dao;
        private readonly IEventBus _eventBus;

        public EmployeeService(IEmployeeDao dao, IEventBus bus)
        {
            _dao = dao;
            _eventBus = bus;
        }
        
        public void InitializeTable() => _dao.InitializeTable();
        public bool HasData() => _dao.HasData();
        public IEnumerable<EmployeeDto> GetAll() => _dao.GetAllEmployees();

        public EmployeeDto? GetById(int id) => _dao.GetEmployeeById(id);

        public bool Add(EmployeeDto dto)
        { 
            var success = _dao.Insert(dto);
            
            if (success)
            {
                _eventBus.Publish(new EmployeeAdded());
            }
            
            return success;
        
        }

        public bool Update(EmployeeDto dto)
        {
            var success = _dao.Update(dto);
            
            if (success)
            {
                _eventBus.Publish(new EmployeeAdded());
            }
            
            return success;
        }

        public bool Delete(int id)
        {
            var success = _dao.Delete(id);
            
            if (success)
            {
                _eventBus.Publish(new EmployeeAdded());
            }
            
            return success;
        } 
        
    }
}
