using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using LoggingServices.LogItems.Items;
using LoggingServices.LogRepositories.Interfaces;

namespace LoggingServices.LogRepositories.Repositories
{
    public class UserActivityRepository :IRepository<UserActivityLogItem>
    {
        private readonly string _connectionString;
        public UserActivityRepository(string connectionString)
        {
            if(String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            this._connectionString = connectionString;
        }
        public List<UserActivityLogItem> ReadAll()
        {
            throw new NotImplementedException();
        }

        public List<UserActivityLogItem> ReadAll(int userId)
        {
            throw new NotImplementedException();
        }

        public List<UserActivityLogItem> ReadAll(string activity)
        {
            throw new NotImplementedException();
        }

        public List<UserActivityLogItem> ReadAll(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public UserActivityLogItem CreateLogItem(UserActivityLogItem logItem)
        {
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                try
                {
                    string createLogItemQuery = "INSERT INTO EIC.dbo.UserActivityLogs " +
                                                "(TimeStamp, Activity, LogType, ImplicatedItemId, UserId) " +
                                                "VALUES (@TimeStamp, @Activity, @LogType, @ImplicatedItemId, @UserId); " +
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
    }
}
