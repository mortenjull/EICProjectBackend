using Domain.DBEntities.Entities;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.ActivityHandlers;
using EICProjectBackend.Handlers.TitleHandlers;
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
    public class ReadAllTitlesTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IRepository<DBTitle>> _titleRepository;
        public List<DBTitle> list;


        public ReadAllTitlesTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _titleRepository = new Mock<IRepository<DBTitle>>();


            DBTitle title1 = new DBTitle()
            {
                Id = 1,
                Created = DateTime.Now,
                Title = "heyij"
            };
            DBTitle title2 = new DBTitle()
            {
                Id = 1,
                Created = DateTime.Now,
                Title = "heyij"
            };
            DBTitle title3 = new DBTitle()
            {
                Id = 1,
                Created = DateTime.Now,
                Title = "heyij"
            };
           
            list = new List<DBTitle>();
            list.Add(title1);
            list.Add(title2);
            list.Add(title3);
        }

        [Fact]
        public async void ReturnListOfTitle()
        {
            _unitOfWork.Setup(mock => mock.TitleRepository.ReadAll())
          .Returns(Task.FromResult(list));

            var command = new ReadAllTitleCommand();
            var handler = new ReadAllTitleHandler(_unitOfWork.Object);
            List<DBTitle> returnValue = (List<DBTitle>)await handler.Handle(command, new CancellationToken());

            Assert.NotNull(returnValue);
            Assert.Equal(3, returnValue.Count);
        }


        [Fact]
        public async void ReturnNull_IfExceptionOccurs()
        {
            var exc = new Exception();
            _unitOfWork.Setup(mock => mock.TitleRepository.ReadAll())
           .Throws(exc);
            var command = new ReadAllTitleCommand();

            var handler = new ReadAllTitleHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.Null(returnValue);
        }

        [Fact]
        public async void ReturnFalse_IfResultIsNull()
        {
            List<DBTitle> list = null;
            _unitOfWork.Setup(mock => mock.TitleRepository.ReadAll())
           .Returns(Task.FromResult(list));
            var command = new ReadAllTitleCommand();

            var handler = new ReadAllTitleHandler(_unitOfWork.Object);
            var returnValue = await handler.Handle(command, new CancellationToken());

            Assert.Null(returnValue);

        }
    }
}
