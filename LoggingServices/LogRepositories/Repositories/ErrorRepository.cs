using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LoggingServices.LogItems.Items;
using LoggingServices.LogRepositories.Interfaces;

namespace LoggingServices.LogRepositories.Repositories
{
    public class ErrorRepository : IRepository<ErrorLogItem>
    {
        private readonly string _connectionString;
        public ErrorRepository(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            this._connectionString = connectionString;
        }

        public ErrorLogItem CreateLogItem(ErrorLogItem logItem)
        {
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                try
                {
                    string createLogItemQuery = "INSERT INTO EIC.dbo.ErrorLogs " +
                                                "(TimeStamp, Activity, LogType, Area, ErrorMessage) " +
                                                "VALUES (@TimeStamp, @Activity, @LogType, @Area, @ErrorMessage); " +
                                                "SELECT SCOPE_IDENTITY();";
                    int result = conn.ExecuteScalar<int>(createLogItemQuery, logItem);
                    if (result <= 0)
                        return null;

                    logItem.Id = result;
                    return logItem;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public bool DeleteLogItem(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLogItems(int userId)
        {
            throw new NotImplementedException();
        }

        public List<ErrorLogItem> ReadAll()
        {
            throw new NotImplementedException();
        }

        public List<ErrorLogItem> ReadAll(int userId)
        {
            throw new NotImplementedException();
        }

        public List<ErrorLogItem> ReadAll(string activity)
        {
            throw new NotImplementedException();
        }

        public List<ErrorLogItem> ReadAll(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
