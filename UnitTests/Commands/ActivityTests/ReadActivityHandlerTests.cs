using Domain.DBEntities.Entities;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.ActivityHandlers;
using Infrastructur.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands2.ActivityTests
{
    public class ReadActivityHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IActivityRepository<Activity>> _activityRepository;
        

        public ReadActivityHandlerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _activityRepository = new Mock<IActivityRepository<Activity>>();

        }

        [Fact]
        public async void ReturnActivity()
        {
            var activity1 = new Activity()
            {
                Created = DateTime.Now,
                Id = 1
            };
            _unitOfWork.Setup(mock => mock.ActivityRepository.Read(It.IsAny<int>()))
           .Returns(Task.FromResult(activity1));

            var command = new ReadActivityCommand(1);
            var handler = new ReadActivityHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(returnValue);
            Assert.Equal(activity1, returnValue);
        }

        [Fact]
        public async void ReturnArgumentOutOfRangeException_IfNegativeId()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadActivityCommand(-1));
        }


        [Fact]
        public async void ReturnFalse_IfExceptionOccurs()
        {
            var exc = new Exception();
            _unitOfWork.Setup(mock => mock.ActivityRepository.Read(It.IsAny<int>()))
           .Throws(exc);
            var command = new ReadActivityCommand(10);

            var handler = new ReadActivityHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.False((bool)returnValue);
        }


        
    }
}
