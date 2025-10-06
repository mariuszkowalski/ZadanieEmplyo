using DataAccess.Models;


namespace DataAccess.DataTransfer.Interfaces
{
    public interface IVacationDao
    {
        void InitializeTable();
        bool HasData();
        bool Delete(int id);
        IEnumerable<VacationDto> GetAllVacations();
        VacationDto? GetVacationById(int id);
        bool Insert(VacationDto dto);
        bool Update(VacationDto dto);
    }
}
