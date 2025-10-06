using Dapper;
using DataAccess.DataTransfer.Interfaces;
using DataAccess.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Data;


namespace DataAccess.DataTransfer
{
    public class TeamDao : BaseDao, ITeamDao
    {
        private const string CacheKeyAll = "Team_All";
        private readonly TimeSpan CacheDurationAll = TimeSpan.FromHours(24);

        public TeamDao(IDbConnection con, ILogger<TeamDao> log, IMemoryCache cache) : base(con, log, cache)
        {
            InitializeTable();
        }

        public override void InitializeTable()
        {
            var sql = @"
            CREATE TABLE IF NOT EXISTS Team (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name VARCHAR(255) UNIQUE
            );";

            ExecuteNonQuery(sql);

            _log.LogDebug("Team table initialized.");
        }

        private static string GetCacheKeyForTeam(int id) => $"Team_{id}";

        public override bool HasData()
        {
            var count = _con.ExecuteScalar<int>($"SELECT COUNT(1) FROM {this.GetType().Name.Replace("Dao", "")};");

            return count > 0;
        }

        public IEnumerable<TeamDto> GetAllTeams()
        {
            if (_cache.TryGetValue(CacheKeyAll, out IEnumerable<TeamDto> dto))
            {
                return dto;
            }

            dto = ExecuteQuery<TeamDto>("SELECT * FROM Team");

            _cache.Set(CacheKeyAll, dto, CacheDurationAll);

            return dto;
        }

        public TeamDto? GetTeamById(int id)
        {
            var cacheKey = GetCacheKeyForTeam(id);

            if (_cache.TryGetValue(cacheKey, out TeamDto dto))
            {
                return dto;
            }

            dto = ExecuteQuerySingle<TeamDto>("SELECT * FROM Team WHERE id=@Id", new { Id = id });

            if (dto != null)
            {
                _cache.Set(cacheKey, dto, CacheDurationAll);
            }

            return dto;
        }

        public bool Insert(TeamDto dto)
        {
            var sql = "INSERT INTO Team (name) VALUES (@Name)";
            ExecuteNonQuery(sql, dto);

            _cache.Remove(CacheKeyAll);

            return true;
        }

        public bool Update(TeamDto dto)
        {
            var sql = "UPDATE Team SET name=@Name WHERE id=@Id";
            ExecuteNonQuery(sql, dto);

            _cache.Remove(CacheKeyAll);
            _cache.Remove(GetCacheKeyForTeam(dto.Id));

            return true;
        }

        public bool Delete(int id)
        {
            var sql = "DELETE FROM Team WHERE id=@Id";
            ExecuteNonQuery(sql, new { Id = id });

            _cache.Remove(CacheKeyAll);
            _cache.Remove(GetCacheKeyForTeam(id));

            return true;
        }
    }
}
