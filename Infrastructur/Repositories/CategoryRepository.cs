using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.DBEntities;

namespace Infrastructur.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public CategoryRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }
        public Category Create(Category entity)
        {
            try
            {
                string createCategoryQuery = "INSERT INTO EIC.dbo.CATEGORY (CategoryName) " +
                                             "VALUES (@CategoryName); " +
                                             "SELECT SCOPE_IDENTITY();";
                int result = this._connection.ExecuteScalar<int>(createCategoryQuery, entity, this._transaction);
                if(result <= 0)
                    return null;

                entity.Id = result;
                return entity;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Category> Read(int id)
        {
            try
            {
                string readCategoryQuery = "SELECT * FROM EIC.dbo.CATEGORY " +
                                           "WHERE Id = " + id + ";";

                var result = await this._connection.QueryAsync<Category>(readCategoryQuery, null, this._transaction);
                if (result == null)
                    return null;

                return result.First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Category>> ReadAll()
        {
            try
            {
                string readAllCategoryQuery = "SELECT * FROM EIC.dbo.CATEGORY; ";
                                              

                var result = await this._connection.QueryAsync<Category>(readAllCategoryQuery, null, this._transaction);
                if (result == null)
                    return null;

                return result.ToList();
            }
            catch (Exception)
            { 
                return null;
            }
        }

        public Category Update(Category entity)
        {
            try
            {
                string updateCategoryQuery = "UPDATE EIC.dbo.CATEGORY " +
                                             "SET CategoryName = @CategoryName " +
                                             "WHERE Id = @Id";

                var result = this._connection.Execute(updateCategoryQuery, entity, this._transaction);
                if (result <= 0)
                    return null;

                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                string deleteCategoryQuery = "DELETE FROM EIC.dbo.CATEGORY " +
                                             "WHERE Id = " + id + ";";

                var result = this._connection.Execute(deleteCategoryQuery, null, this._transaction);
                if (result == null || result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
