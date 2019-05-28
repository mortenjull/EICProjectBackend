using Domain.DBEntities.Entities;
using Domain.ModelEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DBEntities;

namespace Infrastructur.Repositories
{
    public interface IActivityRepository<T> : IRepository<T> where T : IEntity
    {
        /// <summary>
        ///  Creates activityresearcher
        /// </summary>
        /// <returns>True if created</returns>
        bool CreateActivityResearcher(ActivityResearcher model);
        /// <summary>
        ///  Deletes activityresearcher
        /// </summary>
        /// <returns>True if deleted</returns>
        bool DeleteActivityResearcher(int id);
        /// <summary>
        ///  read all activities for researcher
        /// </summary>
        /// <returns>list of activities for researcher</returns>
        Task<List<ActivityWithUserModel>> ReadAllActivityForResearcher(int id);
        /// <summary>
        /// Deletes ActivityResearcher via researcherId
        /// </summary>
        /// <param name="id">Uniqe identifier of reseacher</param>
        /// <returns>Ture or false</returns>
        bool DeleteActivityReseacherViaResearcherId(int id);        
    }
}
