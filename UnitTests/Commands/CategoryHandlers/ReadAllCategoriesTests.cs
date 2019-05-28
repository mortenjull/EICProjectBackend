using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.DBEntities;
using EICProjectBackend.Handlers.CategoryHandlers;
using EICProjectBackend.Services;
using Moq;
using UnitOfWork;
using Xunit;

namespace UnitTests.Commands.CategoryHandlers
{
    public class ReadAllCategoriesTests
    {
        private readonly Mock<IUnitOfWork> _unitOfUnitMock;
        
        public ReadAllCategoriesTests()
        {
            this._unitOfUnitMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void Handle_CategorisNul_Test()
        {
            List<Category> categoriesNull = null;
            this._unitOfUnitMock.Setup(mock => mock.CategoryRepository.ReadAll())
                .Returns(Task.FromResult(categoriesNull));

            ReadAllCategoriesCommand command = new ReadAllCategoriesCommand();
            ReadAllCategoriesCommandHanler handler = new ReadAllCategoriesCommandHanler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }

        [Fact]
        public async void Handle_ReadAllCategories_Test()
        {
            List<Category> categories = new List<Category>();
            this._unitOfUnitMock.Setup(mock => mock.CategoryRepository.ReadAll())
                .Returns(Task.FromResult(categories));

            ReadAllCategoriesCommand command = new ReadAllCategoriesCommand();
            ReadAllCategoriesCommandHanler handler = new ReadAllCategoriesCommandHanler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Equal(categories, result);
        }

        [Fact]
        public async void Handle_Exception_Test()
        {
            var ex = new Exception();
            this._unitOfUnitMock.Setup(mock => mock.CategoryRepository.ReadAll())
                .Throws(ex);

            ReadAllCategoriesCommand command = new ReadAllCategoriesCommand();
            ReadAllCategoriesCommandHanler handler = new ReadAllCategoriesCommandHanler(this._unitOfUnitMock.Object);

            var result = await handler.Handle(command, new CancellationTokenSource().Token);

            Assert.Null(result);
        }
    }
}
