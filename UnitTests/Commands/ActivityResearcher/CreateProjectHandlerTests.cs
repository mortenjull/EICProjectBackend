using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UnitOfWork;

namespace UnitTests.Commands2
{
    public class CreateProjectHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public CreateProjectHandlerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
        }
    }
}
