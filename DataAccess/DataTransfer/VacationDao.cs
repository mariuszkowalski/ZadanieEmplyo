using Dapper;
using DataAccess.DataTransfer.Interfaces;
using DataAccess.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DataAccess.DataTransfer
{
    public class VacationDao : BaseDao, IVacationDao
    {
        private const string CacheKeyAll = "Vacation_All";
        private readonly TimeSpan CacheDurationAll = TimeSpan.FromMinutes(1);

        public VacationDao(IDbConnection con, ILogger<VacationDao> log, IMemoryCache cache)
            : base(con, log, cache)
        {
            InitializeTable();
        }

        public override void InitializeTable()
        {
            var sql = @"
            CREATE TABLE IF NOT EXISTS Vacation (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                dateSince DATETIME,
                dateUntil DATETIME,
                numberOfHours INTEGER,
                isPartialVacation INTEGER,
                employeeId INTEGER
            );
            CREATE INDEX IF NOT EXISTS idx_vacation_employeeId
                ON Vacation (employeeId);";

            ExecuteNonQuery(sql);

            _log.LogDebug("Vacation table initialized.");
        }

        private static string GetCacheKeyForVacation(int id) => $"Vacation_{id}";

        public override bool HasData()
        {
            var count = _con.ExecuteScalar<int>($"SELECT COUNT(1) FROM {this.GetType().Name.Replace("Dao", "")};");

            return count > 0;
        }

        public IEnumerable<VacationDto> GetAllVacations()
        {
            if (_cache.TryGetValue(CacheKeyAll, out IEnumerable<VacationDto> vacations))
            {
                return vacations;
            }

            vacations = ExecuteQuery<VacationDto>("SELECT * FROM Vacation");
            _cache.Set(CacheKeyAll, vacations, CacheDurationAll);
            
            return vacations;
        }

        public VacationDto? GetVacationById(int id)
        {
            var cacheKey = GetCacheKeyForVacation(id);

            if (_cache.TryGetValue(cacheKey, out VacationDto dto))
            {
                return dto;
            }

            dto = ExecuteQuerySingle<VacationDto>("SELECT * FROM Vacation WHERE id=@Id", new { Id = id });

            if (dto != null)
            {
                _cache.Set(cacheKey, dto, CacheDurationAll);
            }

            return dto;
        }

        public bool Insert(VacationDto dto)
        {
            var sql = @"INSERT INTO Vacation (dateSince, dateUntil, numberOfHours, isPartialVacation, employeeId)
                                      VALUES (@DateSince, @DateUntil, @NumberOfHours, @IsPartialVacation, @EmployeeId)";
            
            ExecuteNonQuery(sql, dto);

            _cache.Remove(CacheKeyAll);

            return true;
        }

        public bool Update(VacationDto dto)
        {
            var sql = @"UPDATE Vacation
                           SET dateSince=@DateSince, dateUntil=@DateUntil, numberOfHours=@NumberOfHours,
                               isPartialVacation=@IsPartialVacation, employeeId=@EmployeeId
                         WHERE id=@Id";
            
            ExecuteNonQuery(sql, dto);

            _cache.Remove(CacheKeyAll);
            _cache.Remove(GetCacheKeyForVacation(dto.Id));

            return true;
        }

        public bool Delete(int id)
        {
            var sql = "DELETE FROM Vacation WHERE id=@Id";
            ExecuteNonQuery(sql, new { Id = id });

            _cache.Remove(CacheKeyAll);
            _cache.Remove(GetCacheKeyForVacation(id));

            return true;
        }
    }
}
