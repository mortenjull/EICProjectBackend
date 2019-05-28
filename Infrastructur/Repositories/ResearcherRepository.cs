using Dapper;
using Domain.DBEntities;
using Domain.DBEntities.Entities;
using Domain.ModelEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructur.Repositories
{
    public class ResearcherRepository : IResearcherRepository<Researcher>
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
         
        public ResearcherRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        public Researcher Create(Researcher researcher)
        {
            try
            {
                string createResearcherQuery = "INSERT INTO EIC.dbo.Researcher (Created, ResearcherName, EICColab, Mail, Phone) " +
                                            "VALUES (@Created, @ResearcherName, @EICColab, @Mail, @Phone);" +
                                            "SELECT SCOPE_IDENTITY();";

                var result = this._connection.ExecuteScalar<int>(createResearcherQuery, researcher, this._transaction);
                if (result <= 0)
                    return null;

                researcher.Id = result;
                return researcher;
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
                string deleteResearcherQuery = "DELETE FROM EIC.dbo.Researcher " +
                                               "WHERE Id = " + id + ";";

                var result = this._connection.Execute(deleteResearcherQuery, null, this._transaction);
                if (result <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Researcher> Read(int id)
        {
            try
            {
                string readResearchertQuery = "SELECT * FROM EIC.dbo.Researcher " +
                                          "WHERE Id = " + id + ";";

                var result = await this._connection.QueryAsync<Researcher>(readResearchertQuery, null, this._transaction);
                if (result == null || !result.Any())
                    return null;

                return result.First();
            }
            catch (Exception)            {
                return null;
            }
        }

        public async Task<List<Researcher>> ReadAll()
        {
            try
            {
                string readAllResearchersQuery = "SELECT * FROM EIC.dbo.RESEARCHER ORDER BY ResearcherName ASC;";

                var result = await this._connection.QueryAsync<Researcher>(readAllResearchersQuery, null, this._transaction);
                if (result == null)
                    return null;

                return result.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
         
        public Researcher Update(Researcher researcher)
        {
            try
            {
                string updateResearcherQuery = "UPDATE EIC.dbo.Researcher " +
                                            "SET Phone = @Phone, " +
                                            "ResearcherName = @ResearcherName, " +
                                            "EICColab = @EICColab, " +
                                            "Mail = @Mail " +
                                            "WHERE Id = @Id;";
                var result = this._connection.Execute(updateResearcherQuery, researcher, this._transaction);
                if (result <= 0)
                    return null;

                return researcher;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Researcher>> ReadAllResearcherWithCategories()
        {
            try
            {
                var lookup = new List<Researcher>();
                var result = await this._connection.QueryAsync<Researcher, Category, Researcher>("usp_ReadAllResearcherWithCategories", (researcher, category) => {
                    // Find the researcher in the lookup list
                    var r = lookup.FirstOrDefault(c => c.Id == researcher.Id);
                    if (r == null)
                    {
                        lookup.Add(researcher);
                        r = researcher;
                    }

                    //check if company has a list of categories
                    r.Categories = r.Categories ?? new List<Category>();
                    // only add if the category actual is there, and its not already present
                    if (category != null && !r.Categories.Any(cat => cat.CategoryName == category.CategoryName))
                    {
                        r.Categories.Add(category);
                    }
                    return null;
                }, transaction: this._transaction, commandType: CommandType.StoredProcedure);
                return lookup;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Researcher> ReadResearcherWithCategories(int id)
        {
            try
            {
                string readResearchertQuery = "SELECT Researcher.Id,  Researcher.ResearcherName, Researcher.EICColab, Researcher.Mail, Researcher.Phone as Phone, Category.Id, Category.CategoryName FROM Researcher LEFT JOIN ResearcherCategory on Researcher.Id = ResearcherCategory.ResearcherId LEFT JOIN Category on ResearcherCategory.CategoryId = Category.Id WHERE Researcher.id = " + id;

                Researcher tempResearcher = null;
                var result = await this._connection.QueryAsync<Researcher, Category, Researcher>(readResearchertQuery, (researcher, category) => {
                    if (tempResearcher == null)
                    {
                        tempResearcher = researcher;
                    }

                    //check if researcher has a list of categories
                    tempResearcher.Categories = tempResearcher.Categories ?? new List<Category>();
                    // only add if the category actual is there, and its not already present
                    if (category != null && !tempResearcher.Categories.Any(cat => cat.CategoryName == category.CategoryName))
                    {
                        tempResearcher.Categories.Add(category);
                    }
                    return null;
                }, transaction: this._transaction);
                              return tempResearcher;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        public async Task<Researcher> ReadResearcherWithEverything(int id)
        { 
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", id);
                    Researcher tempResearcher = null;
                    var result = await this._connection.QueryAsync<Researcher, Category, Organisation, Organisation, Project, DBTitle, Researcher>("USP_ReadResearcherWithEverythng", (researcher, category, organisation, mainOrganisation, project, title) => {
                        if (tempResearcher == null)
                        {
                            tempResearcher = researcher;
                        }

                        //Categories
                        tempResearcher.Categories = tempResearcher.Categories ?? new List<Category>();
                        // only add if the category actual is there, and its not already present
                        if (category != null && !tempResearcher.Categories.Any(cat => cat.CategoryName == category.CategoryName))
                        {
                            tempResearcher.Categories.Add(category);
                        }
                        
                        //Organisations
                        tempResearcher.Organisations = tempResearcher.Organisations ?? new List<Organisation>();
                        if (organisation != null && !tempResearcher.Organisations.Any(orga => orga.OrganisationName == organisation.OrganisationName))
                        {
                            tempResearcher.Organisations.Add(organisation);
                        }
                        if (mainOrganisation != null && !tempResearcher.Organisations.Any(orga => orga.Id == mainOrganisation.Id))
                        {
                            tempResearcher.Organisations.Add(mainOrganisation);
                        }

                        //Projects
                        tempResearcher.Projects = tempResearcher.Projects ?? new List<Project>(); 
                        if (project != null && !tempResearcher.Projects.Any(pro => pro.ProjectName == project.ProjectName))
                        {
                            tempResearcher.Projects.Add(project);
                        }

                        //Titles
                        tempResearcher.Titles = tempResearcher.Titles ?? new List<DBTitle>();
                        if (title != null && !tempResearcher.Titles.Any(item => item.Title == title.Title))
                        {
                            tempResearcher.Titles.Add(title);
                        }

                        return null;
                    }, param: parameters, transaction: this._transaction, commandType: CommandType.StoredProcedure);
                    return tempResearcher;
                }
                catch (Exception e)
                {
                    return null;
                }
        }

        public async Task<List<Researcher>> ReadResearchersWithEverything()
        {
            try
            {
                var lookup = new List<Researcher>();
                var result = await this._connection.QueryAsync<Researcher, Category, Organisation, Organisation, Project, DBTitle, Researcher>("USP_ReadReseachersWithEverything", (researcher, category, organisation, mainOrganisation, project, title) => {

                    var tempResearcher = lookup.FirstOrDefault(c => c.Id == researcher.Id);
                    if (tempResearcher == null)
                    {
                       
                        lookup.Add(researcher);
                        tempResearcher = researcher;
                    }
                    
                    //Categories
                    tempResearcher.Categories = tempResearcher.Categories ?? new List<Category>();
                    // only add if the category actual is there, and its not already present
                    if (category != null && !tempResearcher.Categories.Any(cat => cat.Id == category.Id))
                    {
                        tempResearcher.Categories.Add(category);
                    }

                    //Organisations
                    tempResearcher.Organisations = tempResearcher.Organisations ?? new List<Organisation>();
                    if (organisation != null && !tempResearcher.Organisations.Any(orga => orga.Id == organisation.Id))
                    {
                        tempResearcher.Organisations.Add(organisation);
                    }
                    if (mainOrganisation != null && !tempResearcher.Organisations.Any(orga => orga.Id == mainOrganisation.Id))
                    {
                        tempResearcher.Organisations.Add(mainOrganisation);
                    }

                    //Projects
                    tempResearcher.Projects = tempResearcher.Projects ?? new List<Project>();
                    if (project != null && !tempResearcher.Projects.Any(pro => pro.Id == project.Id))
                    {
                        tempResearcher.Projects.Add(project);
                    }

                    //Titles
                    tempResearcher.Titles = tempResearcher.Titles ?? new List<DBTitle>();
                    if (title != null && !tempResearcher.Titles.Any(item => item.Id == title.Id))
                    {
                        tempResearcher.Titles.Add(title);
                    }

                    return null;
                }, transaction: this._transaction, commandType: CommandType.StoredProcedure);
                return lookup;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
