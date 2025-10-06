using DataAccess.Models;


namespace DataAccess.DataTransfer.Interfaces
{
    public interface IVacationPackageDao
    {
        void InitializeTable();
        bool HasData();
        bool Delete(int id);
        IEnumerable<VacationPackageDto> GetAllPackages();
        VacationPackageDto? GetPackageById(int id);
        bool Insert(VacationPackageDto dto);
        bool Update(VacationPackageDto dto);
    }
}
