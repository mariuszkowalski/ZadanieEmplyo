using DataAccess.Models;

namespace Services.Core.DataOperation.Interfaces
{
    public interface IEmployeeService
    {
        void InitializeTable();
        bool HasData();
        bool Add(EmployeeDto dto);
        bool Delete(int id);
        IEnumerable<EmployeeDto> GetAll();
        EmployeeDto? GetById(int id);
        bool Update(EmployeeDto dto);
    }
}