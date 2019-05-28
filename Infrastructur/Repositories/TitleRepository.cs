using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.DBEntities.Entities;

namespace Infrastructur.Repositories
{
    public class TitleRepository : IRepository<DBTitle>
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public TitleRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        public DBTitle Create(DBTitle entity)
        {
            throw new NotImplementedException();
        }

        public Task<DBTitle> Read(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DBTitle>> ReadAll()
        {
            try
            {
                string readAllQuery = "SELECT * FROM EIC.dbo.Title;";

                var result = await this._connection.QueryAsync<DBTitle>(readAllQuery, null, this._transaction);
                if (result == null)
                    return null;

                return result.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DBTitle Update(DBTitle entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
