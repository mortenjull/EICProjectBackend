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
    public class UpdateActivityHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IActivityRepository<Activity>> _activityRepository;
        
        public UpdateActivityHandlerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _activityRepository = new Mock<IActivityRepository<Activity>>();
        }

        [Fact]
        public async void ReturnNull_InvalidActivity()
        {
            Activity a = null;
            _unitOfWork.Setup(mock => mock.ActivityRepository.Update(It.IsAny<Activity>()))
           .Returns(a);

            var command = new UpdateActivityCommand(new ActivityModel());
            var handler = new UpdateActivityHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.Null(returnValue);
        }

        [Fact]
        public async void ReturnNull_IfExceptionIsThrown()
        {
            Exception a = null;
            _unitOfWork.Setup(mock => mock.ActivityRepository.Update(It.IsAny<Activity>()))
           .Throws(a);

            var command = new UpdateActivityCommand(new ActivityModel());
            var handler = new UpdateActivityHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.Null(returnValue);
        }

        [Fact]
        public async void ReturnActivity_IfSucceesfullyUpdated()
        {
            Activity a = new Activity() {
                ActivityName = "123",
                Id=1

            };
            _unitOfWork.Setup(mock => mock.ActivityRepository.Update(It.IsAny<Activity>()))
           .Returns(a); 
            var tempActivityModel = new ActivityModel()
            {
                ActivityName = "test",
                ActivityId = 1
            };
            var command = new UpdateActivityCommand(tempActivityModel);

            var handler = new UpdateActivityHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(returnValue);
        }
    }
}
