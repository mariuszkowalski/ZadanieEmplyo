using DataAccess.Models;
using Services.Core.DataOperation.Models;


namespace Services.Core.DataOperation.Mappers
{
    public static class EmployeeVacationUsageMapper
    {
        public static EmployeeVacationUsageDto ToDto(this EmployeeVacationUsage model)
        {
            return new EmployeeVacationUsageDto
            {
                EmployeeId = model.EmployeeId,
                Name = model.Name,
                UsedDays = model.UsedDays
            };
        }

        public static EmployeeVacationUsage ToModel(this EmployeeVacationUsageDto dto)
        {
            return new EmployeeVacationUsage
            {
                EmployeeId = dto.EmployeeId,
                Name = dto.Name,
                UsedDays = dto.UsedDays
            };
        }
    }
}
