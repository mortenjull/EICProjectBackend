using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EICProjectBackend.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EICProjectBackend.Controllers
{
    [Route("[controller]/[action]")]
    public class TestController : Controller
    {
        private readonly IMediator _mediater;

        public TestController(IMediator mediater)
        {
            this._mediater = mediater;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return new OkObjectResult("Hit");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult TestWithBody([FromBody] int number)
        {
            return new OkObjectResult("asd");
        }
    }
}