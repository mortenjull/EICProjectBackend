using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Domain.DBEntities;
using Infrastructur.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructur.Repositories
{
    public class UserRepository : IUserRepository<User> 
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public UserRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        public User Create(User entity)
        {
            try
            {               
                string createUserQuery = "INSERT INTO EIC.dbo.[USER] (Username, Password, RoleId) " +
                                         "VALUES (@Username, @Password, @RoleId); " +
                                         "SELECT SCOPE_IDENTITY();";

                int result = this._connection.ExecuteScalar<int>(createUserQuery, entity, this._transaction);
                if (result <= 0)
                    return null;

                entity.Id = result;
                return entity;
            }
            catch (SqlException e)
            {
                if (e.Number == 2627)
                {
                    throw;
                }
                return null;
            }
        }

        public async Task<User> Read(int id)
        {
            try
            {
                string readUserQuery = "SELECT * FROM EIC.dbo.[User] " +
                                          "WHERE Id = " + id + ";";

                var result = await this._connection.QueryAsync<User>(readUserQuery, null, this._transaction);
                if (result == null || !result.Any())
                    return null;

                return result.First();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task<List<User>> ReadAll()
        {
            throw new NotImplementedException();
        }

        public User Update(User entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> ReadViaUserName(string username)
        {
            try
            {
                string readViaUsernameqQuery = "SELECT * FROM EIC.dbo.[User] " +
                                               "WHERE Username = '" + username + "';";

                var result = await _connection.QueryAsync<User>(readViaUsernameqQuery, null, this._transaction);
                if (result == null || !result.Any())
                    return null;

                return result.First();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Role> ReadUserRole(int roleId)
        {
            try
            {
                string readUserRoleQuery = "SELECT * FROM EIC.dbo.[Role] " +
                                           "WHERE Id = " + roleId + ";";

                var result = await _connection.QueryAsync<Role>(readUserRoleQuery, null, this._transaction);
                if (result == null || !result.Any())
                    return null;

                return result.First();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

