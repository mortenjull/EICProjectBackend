using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.ProjectHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EICProjectBackend.Controllers
{
    [Route("[controller]/[action]")]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;
        public ProjectController(IMediator mediator)
        {
            if(mediator == null)
                throw new ArgumentNullException(nameof(mediator));

            this._mediator = mediator;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public IActionResult CreateProject([FromBody] CreateProjectModel model)
        {
            if(model == null)
                return new BadRequestObjectResult("Invalid Model");

            var command = new CreateProjectCommand(model);

            var result = this._mediator.Send(command).Result;
            if(result == null)
                return new BadRequestObjectResult("An Error Ocurred");
            if(result.GetType() == typeof(bool) && (bool) result == false)
                return new BadRequestObjectResult("Could not create project."); 

            return null;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadAllProjects()
        {
            var command = new ReadAllProjectsCommand();
            var result = this._mediator.Send(command).Result;
            if(result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadProject(int id)
        {
            if(id <= 0)
                return new BadRequestObjectResult("Invalid Id");

            var command = new ReadProjectCommand(id);
            var result = this._mediator.Send(command).Result;

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromBody]CreateProjectModel project)
        {
            if (project == null)
                return new BadRequestObjectResult("Invalid Model");

            var command = new UpdateProjectCommand(project);

            var result = await this._mediator.Send(command);

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
        [Authorize(Roles = "Admin, User")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if(id <= 0)
                return new BadRequestObjectResult("invalid id");

            var command = new DeleteProjectCommand(id);
            var result = await this._mediator.Send(command);
            if(result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult("Succes");
        }
    }
}