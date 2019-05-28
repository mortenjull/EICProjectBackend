using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EICProjectBackend.Handlers.TitleHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EICProjectBackend.Controllers
{
    [Route("[controller]/[action]")]
    public class TitleController : Controller
    {
        private readonly IMediator _mediator;

        public TitleController(IMediator mediator)
        {
            if(mediator == null)
                throw new ArgumentNullException(nameof(mediator));

            this._mediator = mediator;
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadAllTitle()
        {
            var command = new ReadAllTitleCommand();
            var result = await this._mediator.Send(command);

            if(result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
        
    }
}