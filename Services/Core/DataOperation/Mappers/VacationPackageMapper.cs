using DataAccess.Models;
using Services.Core.DataOperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.DataOperation.Mappers
{
    public static class VacationPackageMapper
    {
        public static VacationPackageDto ToDto(this VacationPackage model)
        {
            return new VacationPackageDto
            {
                Id = model.Id,
                Name = model.Name,
                GrantedDays = model.GrantedDays,
                Year = model.Year
            };
        }

        public static VacationPackage ToModel(this VacationPackageDto dto)
        {
            return new VacationPackage
            {
                Id = dto.Id,
                Name = dto.Name,
                GrantedDays = dto.GrantedDays,
                Year = dto.Year
            };
        }
    }
}
