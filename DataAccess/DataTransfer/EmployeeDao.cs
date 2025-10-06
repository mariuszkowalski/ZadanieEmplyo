using Dapper;
using DataAccess.DataTransfer.Interfaces;
using DataAccess.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Data;


namespace DataAccess.DataTransfer
{
    public class EmployeeDao : BaseDao, IEmployeeDao
    {
        private const string CacheKeyAll = "Employee_All";
        private readonly TimeSpan CacheDurationAll = TimeSpan.FromMinutes(30);
        private readonly TimeSpan CacheDurationSingle = TimeSpan.FromMinutes(30);

        public EmployeeDao(IDbConnection con, ILogger<EmployeeDao> log, IMemoryCache cache)
            : base(con, log, cache)
        {
            InitializeTable();
        }

        public override void InitializeTable()
        {
            var sql = @"
            CREATE TABLE IF NOT EXISTS Employee (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name VARCHAR(255),
                teamId INTEGER,
                vacationPackage INTEGER
            );
            CREATE INDEX IF NOT EXISTS idx_employee_teamId
                ON Employee (teamId);

            CREATE INDEX IF NOT EXISTS idx_employee_vacationPackage
                ON Employee (vacationPackage);";

            ExecuteNonQuery(sql);

            _log.LogDebug("Employee table initialized.");
        }

        private static string GetCacheKeyForEmployee(int id) => $"Employee_{id}";


        public override bool HasData()
        {
            var count = _con.ExecuteScalar<int>($"SELECT COUNT(1) FROM {this.GetType().Name.Replace("Dao", "")};");

            return count > 0;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees()
        {
            if (_cache.TryGetValue(CacheKeyAll, out IEnumerable<EmployeeDto> employees))
                return employees;

            employees = ExecuteQuery<EmployeeDto>("SELECT * FROM Employee");
            _cache.Set(CacheKeyAll, employees, CacheDurationAll);
            return employees;
        }

        public EmployeeDto? GetEmployeeById(int id)
        {
            var cacheKey = GetCacheKeyForEmployee(id);

            if (_cache.TryGetValue(cacheKey, out EmployeeDto employee))
            {
                return employee;
            }

            employee = ExecuteQuerySingle<EmployeeDto>("SELECT * FROM Employee WHERE id=@Id", new { Id = id });

            if (employee != null)
            {
                _cache.Set(cacheKey, employee, CacheDurationSingle);
            }

            return employee;
        }

        public bool Insert(EmployeeDto dto)
        {
            var sql = @"INSERT INTO Employee (name, teamId, vacationPackage)
                                      VALUES (@Name, @TeamId, @VacationPackage)";

            ExecuteNonQuery(sql, dto);

            _cache.Remove(CacheKeyAll);

            return true;
        }

        public bool Update(EmployeeDto dto)
        {
            var sql = @"UPDATE Employee 
                           SET name=@Name, teamId=@TeamId, vacationPackage=@VacationPackage 
                         WHERE id=@Id";

            ExecuteNonQuery(sql, dto);

            _cache.Remove(CacheKeyAll);
            _cache.Remove(GetCacheKeyForEmployee(dto.Id));

            return true;
        }

        public bool Delete(int id)
        {
            var sql = @"DELETE FROM Employee WHERE id=@Id";

            ExecuteNonQuery(sql, new { Id = id });

            _cache.Remove(CacheKeyAll);
            _cache.Remove(GetCacheKeyForEmployee(id));

            return true;
        }
    }
}
