using System;
using System.Data;
using Domain.DBEntities;
using Infrastructur.Repositories;
using Infrastructur.Data;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using System.Data.SqlClient;
using System.Security.Claims;
using Domain.DBEntities.Entities;
using LoggingServices.LogItems.Activities;
using LoggingServices.LogItems.Interfaces;
using LoggingServices.LogItems.Items;
using LoggingServices.ServiceQueues;
using Microsoft.AspNetCore.Http;

namespace UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;
        private bool disposed = false;

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private IUserRepository<User> _userRepository;
        private IProjectRepository _projectRepository;
        private IRelationsRepository _relationsRepository;
        private IRepository<Category> _categoryRepository;
        private IOrganisationRepository _organisationRepository;
        private IResearcherRepository<Researcher> _researcherRepository;
        private IActivityRepository<Activity> _activityRepository;
        private IRepository<DBTitle> _titleRepository;

        private readonly IBackgroundTaskQueue<IUserActivityLog> _logUserActivityItemQueue;
        private readonly IBackgroundTaskQueue<IErrorLog> _logErrorItemQueue;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnitOfWork(
            ApplicationDbContext context, 
            IBackgroundTaskQueue<IUserActivityLog> logUserActivityItemQueue,
            IBackgroundTaskQueue<IErrorLog> logErrorItemQueue,
            IHttpContextAccessor httpContextAccessor
            )
        {           
            this._context = context;
            this._connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            this._connection.Open();
            this._transaction = this._connection.BeginTransaction();

            this._logUserActivityItemQueue = logUserActivityItemQueue;
            this._logErrorItemQueue = logErrorItemQueue;
            this._httpContextAccessor = httpContextAccessor;
        }
        
        public IUserRepository<User> UserRepository =>
            _userRepository ?? (_userRepository = new UserRepository(this._connection, this._transaction));
        public IResearcherRepository<Researcher> ResearcherRepository =>
            _researcherRepository ?? (_researcherRepository = new ResearcherRepository(this._connection, this._transaction));

        public IProjectRepository ProjectRepository =>
            _projectRepository ?? (_projectRepository = new ProjectRepository(this._connection, this._transaction));

        public IRelationsRepository RelationsRepository =>
            _relationsRepository ?? (_relationsRepository = new RelationsRepository(this._connection, this._transaction));

        public IRepository<Category> CategoryRepository =>
            _categoryRepository ?? (_categoryRepository = new CategoryRepository(this._connection, this._transaction));

        public IOrganisationRepository OrganisationRepository =>
            _organisationRepository ??
            (_organisationRepository = new OrganisationRepository(this._connection, this._transaction));

        public IActivityRepository<Activity> ActivityRepository =>
            _activityRepository ?? (_activityRepository = new ActivityRepository(this._connection, this._transaction));

        public IRepository<DBTitle> TitleRepository =>
            _titleRepository ?? (_titleRepository = new TitleRepository(this._connection, this._transaction));


        public void Commit()
        {
            try
            {
                this._transaction.Commit();
            }
            catch
            {
                this._transaction.Rollback();
            }
            finally
            {
                this._transaction.Dispose();
                this._transaction = this._connection.BeginTransaction();
                this.resetRepositories();
            }
        }

        public void Rollback()
        {
            this._transaction.Rollback();
            Dispose();
        }
        
        private void resetRepositories()
        {
            this._userRepository = null;
            this._categoryRepository = null;
            this._organisationRepository = null;
            this._projectRepository = null;
            this._relationsRepository = null;
            this._researcherRepository = null;
            this._activityRepository = null;
            this._titleRepository = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this._transaction != null)
                    {
                        this._transaction.Dispose();
                        this._transaction = null;
                    }
                    if (_connection != null)
                    {
                        this._connection.Dispose();
                        this._connection = null;
                    }
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void QueueUserActivityLogItem(ActivityTypes.Activities activity, int implicatedItemId)
        {
            //Encounter an error from allowAnynamus accesspoints. User claims in httpContext is null. Temp fix. 
            var userId = 0;
            if (activity == ActivityTypes.Activities.UserLogin || activity == ActivityTypes.Activities.UserCreated)
            {
                userId = implicatedItemId;
            }
            else
            {
                userId = Int32.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value);
            }
                
            this._logUserActivityItemQueue.QueueBackgroundWorkItem(
                new UserActivityLogItem()
                {
                    TimeStamp = DateTime.Now,
                    Activity = ActivityTypes.Activities.UserLogin,
                    LogType = ActivityTypes.LogTypes.UserActivity,
                    ImplicatedItemId = implicatedItemId,                  
                    UserId = userId
                });
        }

        public void QueueErrorLogItem(ActivityTypes.Activities activity, ActivityTypes.Areas Area, string errorMessage)
        {            
            this._logErrorItemQueue.QueueBackgroundWorkItem(
                new ErrorLogItem()
                {
                    Activity = activity,
                    Area = Area,
                    ErrorMessage = errorMessage,
                    LogType = ActivityTypes.LogTypes.Error,
                    TimeStamp = DateTime.Now
                });
        }
    }
}
