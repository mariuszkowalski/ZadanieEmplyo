using DataAccess.Models;
using Microsoft.Extensions.Caching.Memory;
using Services.Core.DataOperation.Interfaces;


namespace Services.Core.DataOperation
{

    public class VacationReportService : IVacationReportService
    {
        private readonly IVacationReportRepository _repo;
        private readonly IMemoryCache _cache;

        private const string CacheKey1 = "NoVacationThisYear";
        private const string CacheKey2 = "VacationUsage";
        private readonly TimeSpan CacheDurationAll = TimeSpan.FromHours(24);

        public VacationReportService(IVacationReportRepository repo, IMemoryCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public IEnumerable<EmployeeDto> GetEmployeesInDotNetTeamWithVacations2019()
        {
            if (_cache.TryGetValue(CacheKey1, out IEnumerable<EmployeeDto> dto))
            {
                return dto;
            }

            dto = _repo.GetEmployeesInDotNetTeamWithVacations2019();

            _cache.Set(CacheKey1, dto, CacheDurationAll);

            return dto;

        }

        public IEnumerable<EmployeeVacationUsageDto> GetVacationUsageThisYear()
        {
            if (_cache.TryGetValue(CacheKey2, out IEnumerable<EmployeeVacationUsageDto> dto))
            {
                return dto;
            }

            dto = _repo.GetVacationUsageThisYear();

            _cache.Set(CacheKey2, dto, CacheDurationAll);

            return dto;
        }

        public void InvalidateVacationUsageCache()
        {
            _cache.Remove(CacheKey1);
            _cache.Remove(CacheKey2);
        }
    }

}
