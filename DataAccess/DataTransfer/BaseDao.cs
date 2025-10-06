using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;
using Newtonsoft.Json;


namespace DataAccess.DataTransfer
{
    public abstract class BaseDao
    {
        protected readonly IDbConnection _con;
        protected readonly ILogger _log;
        protected readonly IMemoryCache _cache;

        protected BaseDao(IDbConnection con, ILogger log, IMemoryCache cache)
        {
            _con = con;
            _log = log;
            _cache = cache;
        }


        public abstract void InitializeTable();

        public abstract bool HasData();

        protected void ExecuteNonQuery(string sql, object? param = null)
        {
            _con.Open();
            using var transaction = _con.BeginTransaction();
            
            try
            {
                _con.Execute(sql, param, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Transaction failed. SQL: {sql}. Params: {CheckAndSerialize(param)}");
                transaction.Rollback();
                
                throw;
            }
            finally
            {
                _con.Close();
            }
        }

        protected IEnumerable<T> ExecuteQuery<T>(string sql, object? param = null)
        {
            _con.Open();
            using var transaction = _con.BeginTransaction();
            
            try
            {
                var result = _con.Query<T>(sql, param, transaction);
                transaction.Commit();
                
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Query failed for DTO {typeof(T).Name}. SQL: {sql}. Params: {CheckAndSerialize(param)}");
                transaction.Rollback();
                
                throw;
            }
            finally
            {
                _con.Close();
            }
        }

        protected T? ExecuteQuerySingle<T>(string sql, object? param = null)
        {
            _con.Open();
            using var transaction = _con.BeginTransaction();
            
            try
            {
                var result = _con.QuerySingleOrDefault<T>(sql, param, transaction);
                transaction.Commit();
                
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"QuerySingle failed for DTO {typeof(T).Name}. SQL: {sql}. Params: {CheckAndSerialize(param)}");
                transaction.Rollback();
                
                throw;
            }
            finally
            {
                _con.Close();
            }
        }

        private string CheckAndSerialize(object? obj)
        {
            if (obj == null) return "null";
            
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };

                return JsonConvert.SerializeObject(obj, settings);
            }
            catch(Exception ex)
            {
                _log.LogError(ex, $"Unhandled exception occured on serialization of {typeof(object).Name}.");
                
                return obj.ToString() ?? "unserializable";
            }
        }
    }
}
