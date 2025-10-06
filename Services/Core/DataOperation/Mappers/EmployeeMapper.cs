using DataAccess.Models;
using Services.Core.DataOperation.Models;


namespace Services.Core.DataOperation.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDto ToDto(this Employee model)
        {
            return new EmployeeDto
            {
                Id = model.Id,
                Name = model.Name,
                TeamId = model.TeamId,
                PositionId = model.PositionId,
                VacationPackage = model.VacationPackage
            };
        }

        public static Employee ToModel(this EmployeeDto dto)
        {
            return new Employee
            {
                Id = dto.Id,
                Name = dto.Name,
                TeamId = dto.TeamId,
                PositionId = dto.PositionId,
                VacationPackage = dto.VacationPackage
            };
        }
    }
}
