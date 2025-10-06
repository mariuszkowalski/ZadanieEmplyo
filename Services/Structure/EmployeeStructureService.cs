using DataAccess.DataTransfer;
using DataAccess.Models;
using Services.Core.DataOperation.Models;

namespace Services.Structure
{
    public class EmployeeStructureService
    {
        private readonly List<EmployeeStructure> _structure;

        public EmployeeStructureService()
        {
            _structure = new List<EmployeeStructure>();
        }

        public List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees)
        {
            _structure.Clear();

            foreach (var emp in employees)
            {
                AddRelationsRecursive(emp, emp.SuperiorId, 1, employees);
            }

            return _structure;
        }

        private void AddRelationsRecursive(Employee employee, int? superiorId, int rank, List<Employee> allEmployees)
        {
            if (superiorId == null)
            {
                return;
            }

            _structure.Add(new EmployeeStructure
            {
                EmployeeId = employee.Id,
                SuperiorId = superiorId.Value,
                Rank = rank
            });

            var superior = allEmployees.FirstOrDefault(employee => employee.Id == superiorId.Value);
            
            if (superior != null && superior.SuperiorId.HasValue)
            {
                AddRelationsRecursive(employee, superior.SuperiorId, rank + 1, allEmployees);
            }
        }

        public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
        {
            return _structure.FirstOrDefault(r => r.EmployeeId == employeeId && r.SuperiorId == superiorId)?.Rank;
        }
    }
}
