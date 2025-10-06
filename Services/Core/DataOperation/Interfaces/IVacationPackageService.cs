using DataAccess.Models;

namespace Services.Core.DataOperation.Interfaces
{
    public interface IVacationPackageService
    {
        void InitializeTable();
        bool HasData();
        bool Add(VacationPackageDto dto);
        bool Delete(int id);
        IEnumerable<VacationPackageDto> GetAll();
        VacationPackageDto? GetById(int id);
        bool Update(VacationPackageDto dto);
    }
}