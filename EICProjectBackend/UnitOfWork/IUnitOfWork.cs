using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using Domain.DBEntities.Entities;
using Infrastructur.Repositories;
using LoggingServices.LogItems.Activities;
using LoggingServices.LogItems.Interfaces;
using LoggingServices.ServiceQueues;

namespace UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {          
        IUserRepository<User> UserRepository { get;}
        IProjectRepository ProjectRepository { get; }
        IRelationsRepository RelationsRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IOrganisationRepository OrganisationRepository { get; }
        IResearcherRepository<Researcher> ResearcherRepository { get; }
        IActivityRepository<Activity> ActivityRepository { get; }
        IRepository<DBTitle> TitleRepository { get;}


        /// <summary>
        /// Commits the changes done via repository instances.
        /// </summary>
        void Commit();
        /// <summary>
        /// Rolls back any changes made via repository instance,
        /// inside the current transaction.
        /// </summary>
        void Rollback();
        /// <summary>
        /// Queues a user activity to this ConcurrentQueue
        /// </summary>
        /// <param name="activity">The activity</param>
        /// <param name="implicatedItemId">Id of the involved item</param>
        void QueueUserActivityLogItem(ActivityTypes.Activities activity, int implicatedItemId);
        /// <summary>
        /// Queues an error item to this ConcurrentQueue
        /// </summary>
        /// <param name="activity">The Activity</param>
        /// <param name="Area">Area of the Appliaktion</param>
        /// <param name="errorMessage">Thrown error message</param>
        void QueueErrorLogItem(ActivityTypes.Activities activity, ActivityTypes.Areas Area, string errorMessage);

    }
}
