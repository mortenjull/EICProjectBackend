using Domain.DBEntities.Entities;
using Domain.ModelEntities;
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

namespace UnitTests.Commands2
{
    public class CreateActivityResearcherHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IActivityRepository<Activity>> _activityRepository;

        public CreateActivityResearcherHandlerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _activityRepository = new Mock<IActivityRepository<Activity>>();
        }

        [Fact]
        public async void ReturnNotNullActivityResearcher()
        {
            var tempActivityResearcher = new ActivityResearcher() {
                ActivityId = 1,
                Created = DateTime.Now,
                ResearcherId = 1,
                UserId = 1,
                Id = 1
            };
            _unitOfWork.Setup(mock => mock.ActivityRepository.CreateActivityResearcher(It.IsAny<ActivityResearcher>()))
           .Returns(true);

            var command = new CreateActivityResearcherCommand(tempActivityResearcher);
            var handler = new CreateActivityResearcherHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.NotNull(returnValue);
        }

        [Fact]
        public async void ReturnFalseIfBadInformation()
        {
            var tempActivityResearcher = new ActivityResearcher()
            {
                ActivityId = -1,
                Created = DateTime.Now,
                ResearcherId = -1,
                UserId = -1,
                Id = -1
            };
            _unitOfWork.Setup(mock => mock.ActivityRepository.CreateActivityResearcher(It.IsAny<ActivityResearcher>()))
           .Returns(false);

            var command = new CreateActivityResearcherCommand(tempActivityResearcher);
            var handler = new CreateActivityResearcherHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.False((bool)returnValue);
        }

        [Fact]
        public async void ReturnExceptionIfBadInformation()
        {
            var test = new Exception("");
            _unitOfWork.Setup(mock => mock.ActivityRepository.CreateActivityResearcher(It.IsAny<ActivityResearcher>()))
           .Throws(test);

            var command = new CreateActivityResearcherCommand(new ActivityResearcher());
            var handler = new CreateActivityResearcherHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.False((bool)returnValue);
        }
    }
}
