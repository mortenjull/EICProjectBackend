using System;
using System.Threading.Tasks;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.ActivityHandlers;
using EICProjectBackend.Handlers.ActivityHandlers.ActivityResearcherHandlers;
using EICProjectBackend.Handlers.ActivityResearcherHandlers.ActivityResearcherHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EICProjectBackend.Controllers
{

    [Route("[controller]/[action]")]
    public class ActivityController : Controller
    {
        private readonly IMediator _mediator;

        public ActivityController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody] ActivityModel model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Invalid model inputs");

            CreateActivityCommand command = new CreateActivityCommand(model);
            try
            {
                var result = this._mediator.Send(command).Result;

                if (result == null)
                    return new BadRequestObjectResult("Something went wrong");
                if (result.GetType() == typeof(bool) && (bool)result == false)
                    return new BadRequestObjectResult("Something went wrong");
                if (result.GetType() == typeof(string))
                    return new BadRequestObjectResult(result);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadAllActivities()
        {
            var command = new ReadAllActivitiesCommand();
            var result = this._mediator.Send(command).Result;
            if (result == null)
                return new BadRequestObjectResult("something went wrong: " + result.ToString());

            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadActivity(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult("id out of bounds");

            var command = new ReadActivityCommand(id);
            var result = this._mediator.Send(command).Result;
            if (result == null)
                return new BadRequestObjectResult("Something went wrong: " + result.ToString());
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong" + result.ToString());
            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult("id out of bounds");

            DeleteActivityCommand command = new DeleteActivityCommand(id);

            var result = await this._mediator.Send(command);

            if (!(bool)result)
                return new BadRequestObjectResult("something went wrong");
            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        public async Task<IActionResult> UpdateActivity([FromBody]ActivityModel model)
        {
            if (model.ActivityName == null || model.ActivityId <= 0)
                return new BadRequestObjectResult("somerhing went wrong");

            UpdateActivityCommand command = new UpdateActivityCommand(model);

            var result = await this._mediator.Send(command);

            if (result == null)
                return new BadRequestObjectResult("Could not update activity");
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> CreateActivityResearcher([FromBody] ActivityResearcher model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Invalid model inputs");

            CreateActivityResearcherCommand command = new CreateActivityResearcherCommand(model);
            try
            {
                var result = await this._mediator.Send(command);

                if (result == null)
                    return new BadRequestObjectResult("Something went wrong");
                if (result.GetType() == typeof(bool) && (bool)result == false)
                    return new BadRequestObjectResult("Something went wrong");
                if (result.GetType() == typeof(string))
                    return new BadRequestObjectResult(result);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete]
        public async Task<IActionResult> DeleteActivityResearcher(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult("id out of bounds");

            DeleteActivityResearcherCommand command = new DeleteActivityResearcherCommand(id);

            var result = await this._mediator.Send(command);

            if (!(bool)result)
                return new BadRequestObjectResult("something went wrong");
            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadAllActivityForResearcher(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult("id out of bounds");

            var command = new ReadAllActivityForResearcherCommand(id);
            var result = this._mediator.Send(command).Result;

            if (result == null)
                return new BadRequestObjectResult("Something went wrong: " + result.ToString());
            if (result.GetType() == typeof(bool) && (bool)result == false)
                return new BadRequestObjectResult("Something went wrong" + result.ToString());

            return new OkObjectResult(result);
        }
        
    }
}