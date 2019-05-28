using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.DBEntities;
using Domain.ModelEntities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Infrastructur.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public ProjectRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        public Project Create(Project project)
        {
            try
            {
                string createProjectQuery = "INSERT INTO EIC.dbo.PROJECT (Created, ProjectName, StartDate, EndDate) " +
                                            "VALUES (@Created, @ProjectName, @StartDate, @EndDate); " +
                                            "SELECT SCOPE_IDENTITY();";

                var result = this._connection.ExecuteScalar<int>(createProjectQuery, project, this._transaction);
                if (result <= 0)
                    return null;

                project.Id = result;
                return project;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Project> Read(int id)
        {
            try
            {
                string readProjectQuery = "SELECT * FROM EIC.dbo.PROJECT " +
                                          "WHERE Id = " + id + ";";

                var result = await this._connection.QueryAsync<Project>(readProjectQuery, null, this._transaction);
                if (result == null || !result.Any())
                    return null;

                return result.First();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Project>> ReadAll()
        {
            try
            {
                string readAllProjectsQuery = "SELECT * FROM EIC.dbo.PROJECT";

                var result = await this._connection.QueryAsync<Project>(readAllProjectsQuery, null, this._transaction);
                if (result == null)
                    return null;

                return result.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Project Update(Project project)
        {
            try
            {
                string updateProjectQuery = "UPDATE EIC.dbo.PROJECT " +
                                            "SET Created = @Created, ProjectName = @ProjectName, " +
                                            "StartDate = @StartDate, EndDate = @EndDate " +
                                            "WHERE Id = @Id;";
                var result = this._connection.Execute(updateProjectQuery, project, this._transaction);
                if (result <= 0)
                    return null;

                return project;
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
                string deleteProjectQuery = "DELETE FROM EIC.dbo.PROJECT " +
                                            "WHERE Id = " + id + ";";

                var result = this._connection.Execute(deleteProjectQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<ProjectReadModel>> ReadAllProjectsWithChildren()
        {
            try
            {
                var results = new List<ProjectReadModel>();

                var result = await _connection.QueryAsync<ProjectModel, OrganisationModel, CategoryModel, ProjectResearcherModel, ProjectReadModel>
                    ("USP_ReadAllProjects",
                    (project, organisation, category, researcher) =>
                    {                            
                        var lookupProject = results.FirstOrDefault(x => x.ProjectModel.ProjectId == project.ProjectId);
                        if (lookupProject == null)
                        {
                            var projectReadModel = new ProjectReadModel();
                            projectReadModel.CategoryModels = projectReadModel.CategoryModels ?? new List<CategoryModel>();
                            projectReadModel.ResearcherModels = projectReadModel.ResearcherModels ?? new List<ProjectResearcherModel>();
                            projectReadModel.OrganisationsModels =
                                projectReadModel.OrganisationsModels ?? new List<OrganisationModel>();

                            projectReadModel.ProjectModel = project;                           
                            results.Add(projectReadModel);
                            lookupProject = projectReadModel;
                        }
                        if (!lookupProject.OrganisationsModels.Any(x => x.OrganisationId == organisation.OrganisationId))
                        {
                            lookupProject.OrganisationsModels.Add(organisation);
                        }

                        if (!lookupProject.CategoryModels.Any(x => x.CategoryId == category.CategoryId))
                        {
                            lookupProject.CategoryModels.Add(category);
                        }

                        if (!lookupProject.ResearcherModels.Any(x => x.ReseacherId == researcher.ReseacherId))
                        {
                            lookupProject.ResearcherModels.Add(researcher);
                        }                       
                                                   
                        return null;
                    }, splitOn: "OrganisationId,CategoryId,ReseacherId", transaction: this._transaction);

                if (results == null)
                    return null;

                return results;
            }
            catch (Exception e)
            {
                return null;

            }
        }

        public async Task<ProjectReadModel> ReadProjectWithChildren(int id)
        {
            try
            {
                var projectReadModel = new ProjectReadModel();
                projectReadModel.CategoryModels = new List<CategoryModel>();
                projectReadModel.OrganisationsModels = new List<OrganisationModel>();
                projectReadModel.ResearcherModels = new List<ProjectResearcherModel>();
                var parameter = new DynamicParameters();
                parameter.Add("@ProjectId", id);

                var response = await _connection.QueryAsync<Project, OrganisationModel, CategoryModel, ProjectResearcherModel, Project>
                    ("USP_ReadProjectWithChildren @ProjectId",
                    (project, organisation, category,  researcherCategory) =>
                    {
                        if (projectReadModel.ProjectModel == null) {
                            var projectModel = new ProjectModel();
                            projectReadModel.ProjectModel = projectModel;
                            projectReadModel.ProjectModel.ProjectId = project.Id;
                            projectReadModel.ProjectModel.ProjectCreated = project.Created;
                            projectReadModel.ProjectModel.ProjectName = project.ProjectName;
                            projectReadModel.ProjectModel.StartDate = project.StartDate;
                            projectReadModel.ProjectModel.EndDate = project.EndDate;
                            projectReadModel.OrganisationsModels = projectReadModel.OrganisationsModels ?? new List<OrganisationModel>();
                            projectReadModel.CategoryModels = projectReadModel.CategoryModels ?? new List<CategoryModel>();
                            projectReadModel.ResearcherModels = projectReadModel.ResearcherModels ?? new List<ProjectResearcherModel>();
                        } 

                        if (researcherCategory != null)
                        {
                            if (!projectReadModel.ResearcherModels.Any(x => x.ProjectResearcherCatId == researcherCategory.ProjectResearcherCatId && x.ReseacherId == researcherCategory.ReseacherId))
                            {
                                projectReadModel.ResearcherModels.Add(researcherCategory);
                            }
                        }

                        if (category != null)
                        {
                            if (!projectReadModel.CategoryModels.Any(x => x.CategoryId == category.CategoryId))
                            {
                                projectReadModel.CategoryModels.Add(category);
                            }
                        }

                        if (organisation != null)
                        {
                            if (!projectReadModel.OrganisationsModels.Any(x => x.OrganisationId == organisation.OrganisationId))
                            {
                                projectReadModel.OrganisationsModels.Add(organisation);
                            }
                        }

                        return null;
                    }, splitOn: "OrganisationId, CategoryId, ReseacherId", transaction: this._transaction, param: parameter);

                if (projectReadModel == null)
                    return null;

                return projectReadModel;
            }
            catch (Exception e)
            {
                return null;

            }
        }
    }
}
