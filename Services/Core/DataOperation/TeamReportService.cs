using DataAccess.DataTransfer.Reports;
using DataAccess.Models;
using Microsoft.Extensions.Caching.Memory;


namespace Services.Core.DataOperation
{
    public class TeamReportService : ITeamReportService
    {
        private readonly ITeamReportRepository _repo;
        private readonly IMemoryCache _cache;

        private const string CacheKey = "TeamWithoutVacation:2019";
        private readonly TimeSpan CacheDurationAll = TimeSpan.FromHours(24);

        public TeamReportService(ITeamReportRepository repo, IMemoryCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public IEnumerable<TeamDto> GetTeamsWithoutVacations2019()
        {
            if (_cache.TryGetValue(CacheKey, out IEnumerable<TeamDto> dto))
            {
                return dto;
            }

            dto = _repo.GetTeamsWithoutVacations2019();

            _cache.Set(CacheKey, dto, CacheDurationAll);

            return dto;
        }

        public void InvalidateVacationUsageCache()
        {
            _cache.Remove(CacheKey);
        }
    }
}
