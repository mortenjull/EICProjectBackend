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
    public class ReadAllActivityHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IActivityRepository<Activity>> _activityRepository;
        public List<Activity> list;


        public ReadAllActivityHandlerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _activityRepository = new Mock<IActivityRepository<Activity>>();
         
            Activity activit1 = new Activity()
            {
                ActivityName = "Tester1",
                Id = 1
            };
            Activity activity2 = new Activity()
            {
                ActivityName = "Tester1",
                Id = 1
            };
            Activity activity3 = new Activity()
            {
                ActivityName = "Tester1",
                Id = 1
            };
            list = new List<Activity>();
            list.Add(activit1);
            list.Add(activity3);
            list.Add(activity2);
        }

        [Fact]
        public async void ReturnListOfActivities()
        {
            _unitOfWork.Setup(mock => mock.ActivityRepository.ReadAll())
          .Returns(Task.FromResult(list));

            var command = new ReadAllActivitiesCommand();
            var handler = new ReadAllActivitiesHandler(_unitOfWork.Object);
            List<Activity> returnValue = (List<Activity>)await handler.Handle(command, new CancellationToken());

            Assert.NotNull(returnValue);
            Assert.Equal(3, returnValue.Count);
        }


        [Fact]
        public async void ReturnNull_IfExceptionOccurs()
        {
            var exc = new Exception();
            _unitOfWork.Setup(mock => mock.ActivityRepository.ReadAll())
           .Throws(exc);
            var command = new ReadAllActivitiesCommand();

            var handler = new ReadAllActivitiesHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.False((bool)returnValue);
        }

        [Fact]
        public async void ReturnFalse_IfResultIsNull()
        {
            List<Activity> list = null;
            _unitOfWork.Setup(mock => mock.ActivityRepository.ReadAll())
           .Returns(Task.FromResult(list));
            var command = new ReadAllActivitiesCommand();

            var handler = new ReadAllActivitiesHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.False((bool)returnValue);
        }


    }
}
