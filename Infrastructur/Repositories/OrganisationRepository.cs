using Domain.DBEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.ModelEntities;

namespace Infrastructur.Repositories
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public OrganisationRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        public Organisation Create(Organisation entity)
        {
            try
            {
                string createOrganisationQuery = "INSERT INTO EIC.dbo.ORGANISATION (Created, OrganisationName, Address, " +
                                                 "MainOrganisationId, IsMainOrganisation, EICColaboration, ZipCode, City, Country) " +
                                                 "VALUES (@Created, @OrganisationName, @Address, @MainOrganisationId, " +
                                                 "@IsMainOrganisation, @EICColaboration, @ZipCode, @City, @Country); " +
                                                 "SELECT SCOPE_IDENTITY(); ";

                var result = this._connection.ExecuteScalar<int>(createOrganisationQuery, entity, this._transaction);
                if (result <= 0)
                    return null;

                entity.Id = result;
                return entity;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                string deleteOrganisationQuery = "DELETE FROM EIC.dbo.ORGANISATION " +
                                                 "WHERE Id = " + id +";";
                var result = this._connection.Execute(deleteOrganisationQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    
        public async Task<Organisation> Read(int id)
        {
            try
            {
                string readOrganisationQuery = "SELECT * FROM EIC.dbo.ORGANISATION " +
                                               "WHERE Id = " + id + ";";

                var result = await this._connection.QueryAsync<Organisation>(readOrganisationQuery, null, this._transaction);
                if (result == null || !result.Any())
                    return null;

                return result.First();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Organisation>> ReadAll()
        {
            try
            {
                string readAllOrganisationsQuery = "SELECT * FROM EIC.dbo.ORGANISATION;";

                var result = await 
                    this._connection.QueryAsync<Organisation>(readAllOrganisationsQuery, null, this._transaction);
                if (result == null)
                    return null;

                return result.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Organisation Update(Organisation entity)
        {
            try
            {
                string updateOrganisationsQuery = "UPDATE EIC.dbo.ORGANISATION " +
                                                  "SET  Created = @Created, OrganisationName = @OrganisationName, Address = @Address, " +
                                                  "MainOrganisationId = @MainOrganisationId, IsMainOrganisation = @IsMainOrganisation, " +
                                                  "EICColaboration = @EICColaboration, ZipCode = @ZipCode, City = @City, Country = @Country " +
                                                  "WHERE Id = @Id; " +
                                                  "SELECT SCOPE_IDENTITY();";

                var result = this._connection.Query(updateOrganisationsQuery, entity, this._transaction);
                if (result == null)
                    return null;

                return entity;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<OrganisationWithChildren> ReadOrganisationWithChildren(int id)
        {
            try
            {
                var result = new OrganisationWithChildren();
                result.Projects = new List<ProjectModel>();
                result.Researchers = new List<ResearcherModel>();
                var parameter = new DynamicParameters();
                parameter.Add("@OrganisationId", id);

                var response = await  _connection.QueryAsync<OrganisationWithChildren, ProjectModel, ResearcherModel, OrganisationWithChildren>
                    ("USP_ReadOrganisationWithChildren @OrganisationId",
                    (organisation, project, researcher) =>
                    {
                        result.Id = organisation.Id;
                        result.Created = organisation.Created;
                        result.OrganisationName = organisation.OrganisationName;
                        result.Address = organisation.Address;
                        result.MainOrganisationId = organisation.MainOrganisationId;
                        result.IsMainOrganisation = organisation.IsMainOrganisation;
                        result.EICColaboration = organisation.EICColaboration;
                        result.ZipCode = organisation.ZipCode;
                        result.City = organisation.City;
                        result.Country = organisation.Country;
                        result.Researchers = result.Researchers ?? new List<ResearcherModel>();
                        result.Projects = result.Projects ?? new List<ProjectModel>();

                        var lookupOrganisation = result;

                        if (project != null)
                        {
                            if (!lookupOrganisation.Projects.Any(x => x.ProjectId == project.ProjectId))
                            {
                                lookupOrganisation.Projects.Add(project);
                            }
                        }

                        if (researcher != null)
                        {
                            if (!lookupOrganisation.Researchers.Any(x => x.ResearcherId == researcher.ResearcherId))
                            {
                                lookupOrganisation.Researchers.Add(researcher);
                            }
                        }
                                               
                        return null;
                    }, splitOn: "ProjectId,ResearcherId", transaction: this._transaction, param: parameter);

                if (result == null)
                    return null;

                return result;
            }
            catch (Exception e)
            {
                return null;

            }
        }

        public async Task<bool> ResetMainOrganisationId(int MainOrganisationId)
        {
            try
            {
                string ResetMainOrganisationId = "UPDATE EIC.dbo.ORGANISATION " +
                                                 "SET MainOrganisationId = NULL, IsMainOrganisation = 1" +
                                                 "WHERE MainOrganisationId = " + MainOrganisationId + ";";
                var result = this._connection.Query(ResetMainOrganisationId, null, this._transaction);
                if (result == null)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
