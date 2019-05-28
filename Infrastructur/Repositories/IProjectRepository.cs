using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DBEntities;
using Domain.ModelEntities;

namespace Infrastructur.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// Reads all Projectsdingding with children attached.
        /// Children: Categories, Researchersdinding, Organisation. 
        /// </summary>
        /// <returns>List ofProjectReadModel</returns>
        Task<List<ProjectReadModel>> ReadAllProjectsWithChildren();

        /// <summary>
        /// Reads a Project with children attached.
        /// Children: Categories, Researchersdinding, Organisation. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProjectReadModel</returns>
        Task<ProjectReadModel> ReadProjectWithChildren(int id);
    }
}
