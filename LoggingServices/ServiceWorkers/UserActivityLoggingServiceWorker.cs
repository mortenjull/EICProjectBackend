using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructur.Data;
using LoggingServices.Loggers.Interfaces;
using LoggingServices.Loggers.LogServices;
using LoggingServices.LogItems.Activities;
using LoggingServices.LogItems.Interfaces;
using LoggingServices.LogItems.Items;
using LoggingServices.LogRepositories.Repositories;
using LoggingServices.ServiceQueues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingServices.ServiceWorkers
{
    public class UserActivityLoggingServiceWorker : BackgroundWorker
    {
        private readonly IBackgroundTaskQueue<IUserActivityLog> _userActivityLogItems;
        private readonly IBackgroundTaskQueue<IErrorLog> _errorLogItems;
        private readonly IServiceScopeFactory scopeFactory;

        public UserActivityLoggingServiceWorker(
            IBackgroundTaskQueue<IUserActivityLog> userActivityLogQueue,
            IBackgroundTaskQueue<IErrorLog> errorLogQueue,
        IServiceScopeFactory scopeFactory)
        {
            if(userActivityLogQueue == null)
                throw new ArgumentNullException(nameof(userActivityLogQueue));
            if(errorLogQueue == null)
                throw new ArgumentNullException(nameof(errorLogQueue));
            if(scopeFactory == null)
                throw new ArgumentNullException(nameof(scopeFactory));

            this._userActivityLogItems = userActivityLogQueue;
            this._errorLogItems = errorLogQueue;
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    string connString = dbContext.Database.GetDbConnection().ConnectionString;
                    var userRepo = new UserActivityRepository(connString);
                    var errorRepo = new ErrorRepository(connString);

                    var errorLogger = new ErrorLoggerService(errorRepo);
                    var userActivityLogger = new UserActivityLogService(
                        userRepo, 
                        errorLogger);

                    
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (await this._errorLogItems.QueueIsEmpty(cancellationToken) &&
                            await this._userActivityLogItems.QueueIsEmpty(cancellationToken))
                        {
                            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                        }
                        else
                        {
                            if (await this._userActivityLogItems.QueueIsEmpty(cancellationToken) == false)
                            {
                                var item = await this._userActivityLogItems.DequeueAsync(cancellationToken);
                                await userActivityLogger.LogThisActivity((UserActivityLogItem)item);

                            }
                            if (await this._errorLogItems.QueueIsEmpty(cancellationToken) == false)
                            {
                                var item = await this._errorLogItems.DequeueAsync(cancellationToken);
                                errorLogger.LogError((ErrorLogItem)item);
                            }
                        }                                            
                    }
                }
                catch (Exception e)
                {
                    StopAsync(cancellationToken);
                }              
            } 
        }
    }
}
