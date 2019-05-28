using Domain.DBEntities.Entities;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.ActivityHandlers;
using EICProjectBackend.Handlers.ActivityHandlers.ActivityResearcherHandlers;
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
    public class CreateActivityHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IActivityRepository<Activity>> _activityRepository;

        public CreateActivityHandlerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _activityRepository = new Mock<IActivityRepository<Activity>>();
        }

        [Fact]
        public async void ReturnNotNullActivity()
        {
            var tempActivity = new Activity()
            {
                ActivityName = "test",Id=1
            };
            var tempActivityModel = new ActivityModel()
            {
                ActivityName = "test",
                ActivityId = 1
            };
            _unitOfWork.Setup(mock => mock.ActivityRepository.Create(It.IsAny<Activity>()))
           .Returns(tempActivity);

            var command = new CreateActivityCommand(tempActivityModel);
            var handler = new CreateActivityHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(returnValue);
        }

        [Fact]
        public async void ReturnFalseIfBadInformation()
        {
            Activity tempActivity = null;
            _unitOfWork.Setup(mock => mock.ActivityRepository.Create(It.IsAny<Activity>()))
           .Returns(tempActivity);

            var command = new CreateActivityResearcherCommand(null);
            var handler = new CreateActivityResearcherHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.False((bool)returnValue);
        }

     
   

        [Fact]
        public async void ReturnArgumentOutOfRangeException_IfNoObject()
        {
            
            var command = new CreateActivityCommand(null);
            var handler = new CreateActivityHandler(_unitOfWork.Object);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => handler.Handle(command, new CancellationToken()));
        }
    }
}
