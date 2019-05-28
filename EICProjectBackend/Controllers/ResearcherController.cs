using System;
using System.Threading.Tasks;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.ResearcherHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EICProjectBackend.Controllers
{
    [Route("[controller]/[action]")]
    public class ResearcherController : Controller
    {
        private readonly IMediator _mediator;

        public ResearcherController(IMediator mediator)
        {
            this._mediator = mediator;

        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> CreateResearcher([FromBody] ResearcherModel model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Invalid model inputs");

            var command = new CreateResearcherCommand(model);
            
            var result = this._mediator.Send(command).Result;

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong");
            if (result.GetType() == typeof(string))
                return new BadRequestObjectResult(result);
            return new OkObjectResult(result);             
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadAllResearchers()
        {
            var command = new ReadAllResearchersCommand();
             var result = this._mediator.Send(command).Result;
            if (result == null)
                return new BadRequestObjectResult("something went wrong: " + result.ToString());

            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadResearcher(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult("id out of bounds");

            var command = new ReadResearcherCommand(id);
            var result = this._mediator.Send(command).Result;
            if (result == null)
                return new BadRequestObjectResult("Something went wrong: " + result.ToString());
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong" + result.ToString());
            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete]
        public async Task<IActionResult> DeleteResearcher(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult("id out of bounds");

            DeleteResearcherCommand command = new DeleteResearcherCommand(id);

            var result = await this._mediator.Send(command);

            if (!(bool)result)
                return new BadRequestObjectResult("something went wrong");
            return new OkObjectResult(result);
        }
        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        public async Task<IActionResult> UpdateResearcher([FromBody]ResearcherModel model)
        {
            if (model.ResearcherName == null && model.ResearcherId <= 0)
                return new BadRequestObjectResult("somerhing went wrong");

            UpdateResearcherCommand command = new UpdateResearcherCommand(model);

            var result = await this._mediator.Send(command);

            if (result == null)
                return new BadRequestObjectResult("Could not update reseacher");
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadResearcherWithCategories(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult("id out of bounds");

            var command = new ReadResearcherWithCategoriesCommand(id);
            var result = this._mediator.Send(command).Result;
            if (result == null)
                return new BadRequestObjectResult("Something went wrong: " + result.ToString());
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong" + result.ToString());
            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadAllResearcherWithCategories()
        { 
            var command = new ReadAllResearcherWithCategoriesCommand();
            var result = this._mediator.Send(command).Result;

            if (result == null)
                return new BadRequestObjectResult("Something went wrong: " + result.ToString());
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong" + result.ToString());

            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadResearcherWithEverything(int id)
        {
            var command = new ReadResearcherWithEverythingCommand(id);
            var result = this._mediator.Send(command).Result;

            if (result == null)
                return new BadRequestObjectResult("Something went wrong: " + result.ToString());
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong" + result.ToString());

            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadAllResearchersWithEverything()
        {
            var command = new ReadAllResearchersWithEverythingCommand();
            var result = this._mediator.Send(command).Result;

            if (result == null)
                return new BadRequestObjectResult("Something went wrong: " + result.ToString());
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong" + result.ToString());

            return new OkObjectResult(result);
        }

    }
}