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
    public class DeleteActivityHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IActivityRepository<Activity>> _activityRepository;

        public List<ActivityWithUserModel> listActivityWithUserModels;

        public DeleteActivityHandlerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _activityRepository = new Mock<IActivityRepository<Activity>>();
        }

        [Fact]
        public async void ReturnNull_InvalidId()
        {
            _unitOfWork.Setup(mock => mock.ActivityRepository.DeleteActivityResearcher(It.IsAny<int>()))
           .Returns(false);

            var command = new DeleteActivityCommand(1);
            var handler = new DeleteActivityHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.False((bool)returnValue);
        }

        [Fact]
        public async void ReturnArgumentOutOfRangeException_IfNegativeId()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DeleteActivityCommand(-1));
        }

        [Fact]
        public async void ReturnFalse_IfExceptionOccurs()
        {
            var exc = new Exception();
            _unitOfWork.Setup(mock => mock.ActivityRepository.ReadAllActivityForResearcher(It.IsAny<int>()))
           .Throws(exc);
            var command = new DeleteActivityCommand(10);

            var handler = new DeleteActivityHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.False((bool)returnValue);
        }
    }
}
