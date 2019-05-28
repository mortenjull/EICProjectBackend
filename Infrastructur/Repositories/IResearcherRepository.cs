using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DBEntities;

namespace Infrastructur.Repositories
{
    public interface IResearcherRepository<T> : IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Read a researcher categories
        /// </summary>
        /// <param name="id">The for the researcher</param>
        /// <returns>Returns the researcher</returns>
        Task<T> ReadResearcherWithCategories(int id);
        /// <summary>
        /// Reads all researchers with categories
        /// </summary> 
        /// <returns>Returns the researchers</returns>
        Task<List<T>> ReadAllResearcherWithCategories();
        /// <summary>
        /// Read a researcher with everything
        /// </summary>
        /// <param name="id">The for the researcher</param>
        /// <returns>Returns the researcher</returns>
        Task<T> ReadResearcherWithEverything(int id);
        /// <summary>
        /// Reads all researchers with everything
        /// </summary>
        /// <returns>Returns list of researchers</returns>
        Task<List<T>> ReadResearchersWithEverything();


    }
}
