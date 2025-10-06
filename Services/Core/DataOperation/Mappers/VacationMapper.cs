using DataAccess.Models;
using Services.Core.DataOperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.DataOperation.Mappers
{
    public static class VacationMapper
    {
        public static VacationDto ToDto(this Vacation model)
        {
            return new VacationDto
            {
                Id = model.Id,
                DateSince = model.DateSince,
                DateUntil = model.DateUntil,
                EmployeeId = model.EmployeeId,
                NumberOfHours = model.NumberOfHours,
                IsPartialVacation = model.IsPartialVacation
            };
        }

        public static Vacation ToModel(this VacationDto dto)
        {
            return new Vacation
            {
                Id = dto.Id,
                DateSince = dto.DateSince,
                DateUntil = dto.DateUntil,
                EmployeeId = dto.EmployeeId,
                NumberOfHours = dto.NumberOfHours,
                IsPartialVacation = dto.IsPartialVacation
            };
        }
    }
}
