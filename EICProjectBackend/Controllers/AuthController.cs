using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ModelEntities;
using EICProjectBackend.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace EICProjectBackend.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Signin([FromBody] AuthSigninModel model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Invalid model inputs");

            var command = new AuthSigninCommand(model);
            var result = this._mediator.Send(command).Result;

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);            
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] AuthSignupModel model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Invalid model inputs");

            var command = new AuthSignupCommand(model);
            var result = this._mediator.Send(command).Result;

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong");
            if (result.GetType() == typeof(string))
                return new BadRequestObjectResult(result);

            return new OkObjectResult(result);
        }



    }
}