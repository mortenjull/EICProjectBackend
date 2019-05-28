using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LoggingServices.Loggers.Interfaces;
using LoggingServices.LogItems.Activities;
using LoggingServices.LogItems.Items;
using LoggingServices.LogRepositories.Interfaces;
using LoggingServices.LogRepositories.Repositories;

namespace LoggingServices.Loggers.LogServices
{
    public class UserActivityLogService : IUserActivityLogger<UserActivityLogItem>
    {
        private readonly IRepository<UserActivityLogItem> _userActivityRepository;
        private readonly IErrorLogger<ErrorLogItem> _errorLoggerService;

        public UserActivityLogService(IRepository<UserActivityLogItem> userActivityRepository, IErrorLogger<ErrorLogItem> errorLoggerService)
        {
            if(userActivityRepository == null)
                throw new ArgumentNullException(nameof(userActivityRepository));
            if(errorLoggerService == null)
                throw new ArgumentNullException(nameof(errorLoggerService));

            this._userActivityRepository = userActivityRepository;
            this._errorLoggerService = errorLoggerService;
        }
        public Task<UserActivityLogItem> DeleteUserActivityLog(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserActivityLogItem> DeleteUserActivityLogs(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserActivityLogItem> LogThisActivity(UserActivityLogItem item)
        {
            try
            {
                var result = this._userActivityRepository.CreateLogItem(item);
                if (result == null)
                {
                    var error = new ErrorLogItem();
                    error.Activity = ActivityTypes.Activities.LogCreated;
                    error.Area = ActivityTypes.Areas.Logging;
                    error.ErrorMessage = "Could not add Logging item";
                    error.LogType = ActivityTypes.LogTypes.Error;
                    error.TimeStamp = DateTime.Now;
                    this._errorLoggerService.LogError(error);
                    return null;
                }

                return result;
            }
            catch (Exception e)
            {
                var error = new ErrorLogItem();
                error.Activity = ActivityTypes.Activities.Error;
                error.Area = ActivityTypes.Areas.Logging;
                error.ErrorMessage = "Could not add Logging item";
                error.LogType = ActivityTypes.LogTypes.Error;
                error.TimeStamp = DateTime.Now;
                this._errorLoggerService.LogError(error);
                return null;
            }
        }

        public List<UserActivityLogItem> ReadAll()
        {
            throw new NotImplementedException();
        }

        public List<UserActivityLogItem> ReadAll(int userId)
        {
            throw new NotImplementedException();
        }

        public List<UserActivityLogItem> ReadAll(ActivityTypes.Activities activity)
        {
            throw new NotImplementedException();
        }

        public List<UserActivityLogItem> ReadAll(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
