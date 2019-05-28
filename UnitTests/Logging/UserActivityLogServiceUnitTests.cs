using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LoggingServices.Loggers.Interfaces;
using LoggingServices.Loggers.LogServices;
using LoggingServices.LogItems.Interfaces;
using LoggingServices.LogItems.Items;
using LoggingServices.LogRepositories.Interfaces;
using LoggingServices.LogRepositories.Repositories;
using Microsoft.WindowsAzure.Storage.Core;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace UnitTests.Logging
{
    public class UserActivityLogServiceUnitTests
    {
        private  UserActivityLogService _userActivityLogService;

        private Mock<IRepository<UserActivityLogItem>> _userActivityRepository;       
        private Mock<IRepository<ErrorLogItem>> _errorLoggerRepository;

        private Mock<IErrorLogger<ErrorLogItem>> _errorLoggerService;

        public UserActivityLogServiceUnitTests()
        {            
            this._userActivityRepository = new  Mock<IRepository<UserActivityLogItem>>();
            this._errorLoggerRepository = new Mock<IRepository<ErrorLogItem>>();

            this._errorLoggerService = new Mock<IErrorLogger<ErrorLogItem>>();

            _userActivityLogService = new UserActivityLogService(
                this._userActivityRepository.Object, this._errorLoggerService.Object);
        }

        [Fact]
        public async void LogThisActivity_Null_Test()
        {
            UserActivityLogItem test = null;           
            this._userActivityRepository.Setup(mock => 
                mock.CreateLogItem(It.IsAny<UserActivityLogItem>())).Returns(test);

            var item = new UserActivityLogItem();

            var result = await this._userActivityLogService.LogThisActivity(item);

            Assert.Null(result);
        }

        [Fact]
        public async void LogThisActivity_Result_Test()
        {
            var item = new UserActivityLogItem();
            this._userActivityRepository.Setup(mock =>
                mock.CreateLogItem(It.IsAny<UserActivityLogItem>())).Returns(item);
            
            var result = await this._userActivityLogService.LogThisActivity(item);

            Assert.Equal(item, result);
        }

        [Fact]
        public async void LogThisActivity_Exception_Test()
        {
            var test = new Exception("",null);

            this._userActivityRepository.Setup(mock =>
                mock.CreateLogItem(It.IsAny<UserActivityLogItem>())).Throws(test);

            var item = new UserActivityLogItem();

            var result = await this._userActivityLogService.LogThisActivity(item);

            Assert.Null(result);
        }
    }
}
