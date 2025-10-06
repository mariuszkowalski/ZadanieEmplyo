using DataAccess.Models;


namespace DataAccess.DataTransfer.Interfaces
{
    public interface IEmployeeDao
    {
        void InitializeTable();
        bool HasData();
        bool Delete(int id);
        IEnumerable<EmployeeDto> GetAllEmployees();
        EmployeeDto? GetEmployeeById(int id);
        bool Insert(EmployeeDto dto);
        bool Update(EmployeeDto dto);
    }
}
