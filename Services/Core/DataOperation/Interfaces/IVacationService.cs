using DataAccess.Models;


namespace Services.Core.DataOperation.Interfaces
{
    public interface IVacationService
    {
        void InitializeTable();
        bool HasData();
        bool Add(VacationDto dto);
        bool Delete(int id);
        IEnumerable<VacationDto> GetAll();
        VacationDto? GetById(int id);
        bool Update(VacationDto dto);
    }
}