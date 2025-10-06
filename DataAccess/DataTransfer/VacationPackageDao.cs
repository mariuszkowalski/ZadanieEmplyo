using Dapper;
using DataAccess.DataTransfer.Interfaces;
using DataAccess.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Data;


namespace DataAccess.DataTransfer
{
    public class VacationPackageDao : BaseDao, IVacationPackageDao
    {
        private const string CacheKeyAll = "VacationPackage_All";
        private readonly TimeSpan CacheDurationAll = TimeSpan.FromHours(24);

        public VacationPackageDao(IDbConnection con, ILogger<VacationPackageDao> log, IMemoryCache cache) : base(con, log, cache)
        {
            InitializeTable();
        }

        public override void InitializeTable()
        {
            var sql = @"
            CREATE TABLE IF NOT EXISTS VacationPackage (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name VARCHAR(255) UNIQUE,
                grantedDays INTEGER,
                year INTEGER
            );";

            ExecuteNonQuery(sql);

            _log.LogDebug("VacationPackage table initialized.");
        }

        private static string GetCacheKeyForPackage(int id) => $"VacationPackage_{id}";

        public override bool HasData()
        {
            var count = _con.ExecuteScalar<int>($"SELECT COUNT(1) FROM {this.GetType().Name.Replace("Dao", "")};");
            
            return count > 0;
        }

        public IEnumerable<VacationPackageDto> GetAllPackages()
        {
            if (_cache.TryGetValue(CacheKeyAll, out IEnumerable<VacationPackageDto> packages))
            {
                return packages;
            }

            packages = ExecuteQuery<VacationPackageDto>("SELECT * FROM VacationPackage");

            _cache.Set(CacheKeyAll, packages, CacheDurationAll);

            return packages;
        }

        public VacationPackageDto? GetPackageById(int id)
        {
            var cacheKey = GetCacheKeyForPackage(id);

            if (_cache.TryGetValue(cacheKey, out VacationPackageDto dto))
            {
                return dto;
            }

            dto = ExecuteQuerySingle<VacationPackageDto>("SELECT * FROM VacationPackage WHERE id=@Id", new { Id = id });

            if (dto != null)
            {
                _cache.Set(cacheKey, dto, CacheDurationAll);
            }

            return dto;
        }

        public bool Insert(VacationPackageDto dto)
        {
            var sql = @"INSERT INTO VacationPackage (name, grantedDays, year)
                                 VALUES (@Name, @GrantedDays, @Year)";

            ExecuteNonQuery(sql, dto);

            _cache.Remove(CacheKeyAll);

            return true;
        }

        public bool Update(VacationPackageDto dto)
        {
            var sql = @"UPDATE VacationPackage
                                 SET name=@Name, grantedDays=@GrantedDays, year=@Year
                                 WHERE id=@Id";
            ExecuteNonQuery(sql, dto);

            _cache.Remove(CacheKeyAll);
            _cache.Remove(GetCacheKeyForPackage(dto.Id));

            return true;
        }

        public bool Delete(int id)
        {
            var sql = "DELETE FROM VacationPackage WHERE id=@Id";
            ExecuteNonQuery(sql, new { Id = id });

            _cache.Remove(CacheKeyAll);
            _cache.Remove(GetCacheKeyForPackage(id));

            return true;
        }
    }
}
