using Domain.DBEntities;
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
    public class ReadAllActivityForResearcherHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IActivityRepository<Activity>> _activityRepository;

        public List<ActivityWithUserModel> listActivityWithUserModels;

        public ReadAllActivityForResearcherHandlerTests() {
            _unitOfWork = new Mock<IUnitOfWork>();
            _activityRepository = new Mock<IActivityRepository<Activity>>();

            var activity1 = new ActivityResearcher()
            {
                ActivityId = 1,
                ResearcherId = 1,
                UserId = 1,
                Created = DateTime.Now,
                Id = 1
            };
            var activity2 = new ActivityResearcher()
            {
                ActivityId = 3,
                ResearcherId = 1,
                UserId = 1,
                Created = DateTime.Now,
                Id = 1
            };
            var activity3 = new ActivityResearcher()
            {
                ActivityId = 2,
                ResearcherId = 1,
                UserId = 1,
                Created = DateTime.Now,
                Id = 1
            };
            var activityList = new List<ActivityWithUserModel>();
            ActivityWithUserModel activityWithUserModel = new ActivityWithUserModel()
            {
                ActivityName = "Tester1",
                Username = "user1",
                Id = 1
            };
            ActivityWithUserModel activityWithUserModel1 = new ActivityWithUserModel()
            {
                ActivityName = "Tester2",
                Username = "user12",
                Id = 2
            };
            ActivityWithUserModel activityWithUserModel2 = new ActivityWithUserModel()
            {
                ActivityName = "Tester3",
                Username = "user3",
                Id = 3
            };
            listActivityWithUserModels = new List<ActivityWithUserModel>();
            listActivityWithUserModels.Add(activityWithUserModel1);
            listActivityWithUserModels.Add(activityWithUserModel2);
            listActivityWithUserModels.Add(activityWithUserModel);
        }

        [Fact]
        public async void ReturnListOfActivityResearcher()
        {
            _unitOfWork.Setup(mock => mock.ActivityRepository.ReadAllActivityForResearcher(It.IsAny<int>()))
           .Returns(Task.FromResult(listActivityWithUserModels));

            var command = new ReadAllActivityForResearcherCommand(1);
            var handler = new ReadAllActivityForResearcherHandler(_unitOfWork.Object);
            List<ActivityWithUserModel> returnValue = (List<ActivityWithUserModel>) await handler.Handle(command, new CancellationToken());

            Assert.NotNull(returnValue);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async void ReturnArgumentOutOfRangeException_IfNegativeId()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadAllActivityForResearcherCommand(-1));
        }
        

        [Fact]
        public async void ReturnFalse_IfExceptionOccurs()
        {
            var exc = new Exception();
            _unitOfWork.Setup(mock => mock.ActivityRepository.ReadAllActivityForResearcher(It.IsAny<int>()))
           .Throws(exc); 
            var command = new ReadAllActivityForResearcherCommand(10);
             
            var handler = new ReadAllActivityForResearcherHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());
            
            Assert.False((bool)returnValue);
        }

        [Fact]
        public async void ReturnFalse_IfResultIsNull()
        {
            List<ActivityWithUserModel> list = null;
            _unitOfWork.Setup(mock => mock.ActivityRepository.ReadAllActivityForResearcher(It.IsAny<int>()))
           .Returns(Task.FromResult(list));
            var command = new ReadAllActivityForResearcherCommand(10);

            var handler = new ReadAllActivityForResearcherHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.False((bool)returnValue);
        }


    }


}
