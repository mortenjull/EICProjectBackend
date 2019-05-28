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
    public class ErrorLoggerService : IErrorLogger<ErrorLogItem>
    {
        private readonly string _connectionString;     
        private readonly IRepository<ErrorLogItem> _errorRepository;

        public ErrorLoggerService(IRepository<ErrorLogItem> errorRepository)
        {
            if (errorRepository == null)
                throw new ArgumentNullException(nameof(errorRepository));
                
            this._errorRepository = errorRepository;
        }

        public List<ErrorLogItem> ReadAll()
        {
            throw new NotImplementedException();
        }

        public List<ErrorLogItem> ReadAll(int userId)
        {
            throw new NotImplementedException();
        }

        public List<ErrorLogItem> ReadAll(ActivityTypes.Activities activity)
        {
            throw new NotImplementedException();
        }

        public List<ErrorLogItem> ReadAll(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public void LogError(ErrorLogItem item)
        {
            try
            {
                this._errorRepository.CreateLogItem(item);                
            }
            catch (Exception e)
            {
                Console.WriteLine("Shit Stuck!!");
            }
        }

        public Task<ErrorLogItem> DeleteUserActivityLog(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorLogItem> DeleteUserActivityLogs(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
