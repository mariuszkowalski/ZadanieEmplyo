using System;
using System.Collections.Generic;
using DataAccess.DataTransfer.Interfaces;
using DataAccess.Models;
using Services.Core.DataOperation.Interfaces;

namespace Services.Core.DataOperation
{
    public class VacationPackageService : IVacationPackageService
    {
        private readonly IVacationPackageDao _dao;

        public VacationPackageService(IVacationPackageDao dao)
        {
            _dao = dao;
        }

        public void InitializeTable() => _dao.InitializeTable();
        public bool HasData() => _dao.HasData();
        public IEnumerable<VacationPackageDto> GetAll() => _dao.GetAllPackages();

        public VacationPackageDto? GetById(int id) => _dao.GetPackageById(id);

        public bool Add(VacationPackageDto dto) => _dao.Insert(dto);

        public bool Update(VacationPackageDto dto) => _dao.Update(dto);

        public bool Delete(int id) => _dao.Delete(id);
    }
}
