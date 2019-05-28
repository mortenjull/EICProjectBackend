using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Domain.DBEntities;
using Domain.DBEntities.RelationEntities;

namespace Infrastructur.Repositories
{
    public class RelationsRepository : IRelationsRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public RelationsRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }
        public OrganisationProject CreateOrganisationProject(OrganisationProject entity)
        {
            try
            {
                string createOrganisationProjectQuery = "INSERT INTO EIC.dbo.PROJECTORGANISATION (Created, ProjectId,                                               OrganisationId) " +
                                                        "VALUES (@Created, @ProjectId, @OrganisationId); " +
                                                        "SELECT SCOPE_IDENTITY();";

                var result =
                    this._connection.ExecuteScalar<int>(createOrganisationProjectQuery, entity, this._transaction);
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

        public bool DeleteOrganisationProject(int id)
        {
            try
            {
                string deleteOrganisationProjectQuery = "DELETE FROM EIC.dbo.PROJECTORGANISATION " +
                                                        "WHERE Id = " + id + ";";

                var result = this._connection.Execute(deleteOrganisationProjectQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAllOrganisationsProjects(int id)
        {
            try
            {
                string deleteOrganisationProjectQuery = "DELETE FROM EIC.dbo.PROJECTORGANISATION " +
                                                        "WHERE OrganisationId = " + id + ";";

                var result = this._connection.Execute(deleteOrganisationProjectQuery, null, this._transaction);              

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateAllOrganisationsProjectsOrganisationId(int oldId, int newId)
        {
            try
            {
                string UpdateQuery = "UPDATE EIC.dbo.ProjectOrganisation " +
                                     "SET OrganisationId = " + newId + " "+
                                     "WHERE OrganisationId = " + oldId + ";";

                var result = this._connection.Query(UpdateQuery, null, this._transaction);
                if (result == null)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public OrganisationResearcher CreateOrganisationResearcher(OrganisationResearcher entity)
        {
            try
            {
                string createOrganisationResearcherQuery = "INSERT INTO EIC.dbo.RESEARCHERORGANISATION (Created,                                                       ResearcherId, OrganisationId, TitleId) " +
                                                           "VALUES (@Created, @ResearcherId, @OrganisationId, @TitleId); " +
                                                           "SELECT SCOPE_IDENTITY();";
                var result =
                    this._connection.ExecuteScalar<int>(createOrganisationResearcherQuery, entity, this._transaction);
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

        public bool DeleteOrganisationResearcher(int reseacherID)
        {
            try
            {
                string deleteOrganisationResearcherQuery = "DELETE FROM EIC.dbo.RESEARCHERORGANISATION " +
                                                           "WHERE ResearcherId = " + reseacherID + ";";

                var result = this._connection.Execute(deleteOrganisationResearcherQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteAllOrganisationResearchers(int id)
        {
            try
            {
                string deleteOrganisationResearcherQuery = "DELETE FROM EIC.dbo.RESEARCHERORGANISATION " +
                                                           "WHERE OrganisationId = " + id + ";";

                var result = this._connection.Execute(deleteOrganisationResearcherQuery, null, this._transaction);               

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateAllAllOrganisationResearchersOrganisationId(int oldId, int newId)
        {
            try
            {
                string UpdateQuery = "UPDATE EIC.dbo.ResearcherOrganisation " +
                                     "SET OrganisationId = " + newId + " " +
                                     "WHERE OrganisationId = " + oldId + ";";

                var result = this._connection.Query(UpdateQuery, null, this._transaction);
                if (result == null)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteProjectOrganisation(int id)
        {
            try
            {
                string deleteProjectOrganisationQuery = "DELETE FROM EIC.dbo.ProjectOrganisation " +
                                                           "WHERE ProjectId = " + id + ";";

                var result = this._connection.Execute(deleteProjectOrganisationQuery, null, this._transaction);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool DeleteCategoriesProject(int id)
        {
            try
            {
                string deleteCategoriesProjectQuery = "DELETE FROM EIC.dbo.ProjectCategory " +
                                                           "WHERE ProjectId = " + id + ";";

                var result = this._connection.Execute(deleteCategoriesProjectQuery, null, this._transaction);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        

        public ProjectCategory CreateProjectCategory(ProjectCategory entity)
        {
            try
            {
                string createProjectCategoryQuery = "INSERT INTO EIC.dbo.PROJECTCATEGORY (Created, ProjectId, CategoryId) " +
                                                    "VALUES (@Created, @ProjectId, @CategoryId); " +
                                                    "SELECT SCOPE_IDENTITY();";

                var result = this._connection.ExecuteScalar<int>(createProjectCategoryQuery, entity, this._transaction);
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

        public bool DeleteProjectCategory(int id)
        {
            try
            {
                string deleteProjectCategoryQuery = "DELETE FROM EIC.dbo.PROJECTCATEGORY " +
                                                    "WHERE Id = " + id + ";";

                var result = this._connection.Execute(deleteProjectCategoryQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public ProjectResearcher CreateProjectResearcher(ProjectResearcher entity)
        {
            try
            {
                string createProjectResearcherQuery = "INSERT INTO EIC.dbo.PROJECTRESEARCHER (Created, ProjectId,                                                 ResearcherId, CategoryId, TitleId) " +
                                                      "VALUES (@Created, @ProjectId, @ResearcherId, @CategoryId, @TitleId);                        " +
                                                      "SELECT SCOPE_IDENTITY();";

                var result = this._connection.ExecuteScalar<int>(createProjectResearcherQuery, entity, this._transaction);
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

        public bool DeleteProjectResearcher(int researcherID)
        {
            try
            {
                string deleteProjectResearcherQuery = "DELETE FROM EIC.dbo.PROJECTRESEARCHER " +
                                                      "WHERE ResearcherId = " + researcherID + ";";

                var result = this._connection.Execute(deleteProjectResearcherQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteProjectResearcherWithProjectId(int id)
        {
            try
            {
                string deleteProjectResearcherQuery = "DELETE FROM EIC.dbo.ProjectResearcher " +
                                                      "WHERE ProjectId = " + id + ";";

                var result = this._connection.Execute(deleteProjectResearcherQuery, null, this._transaction);
                if (result == null)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public ResearcherCategory CreateResearcherCategory(ResearcherCategory entity)
        {

            try
            {
                string createResearcherCategoryQuery = "INSERT INTO EIC.dbo.RESEARCHERCATEGORY (Created, ResearcherId,                                             CategoryId) " +
                                                       "VALUES (@Created, @ResearcherId, @CategoryId); " +
                                                       "SELECT SCOPE_IDENTITY();";

                var result =
                    this._connection.ExecuteScalar<int>(createResearcherCategoryQuery, entity, this._transaction);
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

        public bool DeleteResearcherCategory(int id)
        {
            try
            {
                string deleteResearcherCategoryQuery = "DELETE FROM EIC.dbo.RESEARCHERCATEGORY " +
                                                       "WHERE id = " + id + ";";

                var result = this._connection.Execute(deleteResearcherCategoryQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool DeleteResearcherCategoryViaReseacherID(int researcherId)
        {
            try
            {
                string deleteResearcherCategoryQuery = "DELETE FROM EIC.dbo.RESEARCHERCATEGORY " +
                                                       "WHERE ResearcherId = " + researcherId + ";";

                var result = this._connection.Execute(deleteResearcherCategoryQuery, null, this._transaction);
                if (result <= 0)
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
