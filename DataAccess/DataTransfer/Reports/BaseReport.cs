using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataTransfer.Reports
{
    public abstract class BaseReport
    {
        protected readonly IDbConnection _con;
        protected readonly ILogger _log;

        protected BaseReport(IDbConnection con, ILogger log)
        {
            _con = con;
            _log = log;
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
            catch (Exception ex)
            {
                _log.LogError(ex, $"Unhandled exception occured on serialization of {typeof(object).Name}.");

                return obj.ToString() ?? "unserializable";
            }
        }
    }
}
