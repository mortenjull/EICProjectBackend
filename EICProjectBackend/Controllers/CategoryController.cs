using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EICProjectBackend.Handlers.CategoryHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EICProjectBackend.Controllers
{
    [Route("[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadAllCategories()
        {
            var command = new ReadAllCategoriesCommand();
            var result = this._mediator.Send(command).Result;
            if (result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
    }
}