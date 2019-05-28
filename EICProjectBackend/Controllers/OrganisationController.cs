using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ModelEntities;
using EICProjectBackend.Handlers.OrganisationHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EICProjectBackend.Controllers
{
    [Route("[controller]/[action]")]
    public class OrganisationController : Controller
    {
        private readonly IMediator _mediator;

        public OrganisationController(IMediator mediator)
        {
            if(mediator == null)
                throw new ArgumentNullException(nameof(mediator));

            this._mediator = mediator;
        }
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> CreateOrganisation([FromBody]OrganisationWithChildren organisation)
        {
            if(organisation == null)
                return new BadRequestObjectResult("Invalid Model");
            
            var command = new CreateOrganisationCommand(organisation);

            var result = await this._mediator.Send(command);

            if(result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public IActionResult ReadOrganisation([FromBody] int id)
        {
            return null;
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadAllOrganisations()
        {
            var command = new ReadAllOrganisationsCommand();
            var result = await this._mediator.Send(command);
            if(result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        public async Task<IActionResult> UpdateOrganisation([FromBody]OrganisationWithChildren organisation)
        {
            if (organisation == null)
                return new BadRequestObjectResult("Invalid Model");

            var command = new UpdateOrganisationCommand(organisation);

            var result = await this._mediator.Send(command);

            if (result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
        [Authorize(Roles = "Admin, User")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrganisation(int id)
        {
            if(id <= 0)
                return new BadRequestObjectResult("Invalid Id");

            var command = new DeleteOrganisationCommand(id);
            var result = await this._mediator.Send(command);

            if(result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> ReadOrganisationWithChildren(int id )
        {
            if(id <= 0)
                return new BadRequestObjectResult("Invalid ID");

            var command = new ReadOrganisationWithChildrenCommand(id);
            var result = await this._mediator.Send(command);

            if(result == null)
                return new BadRequestObjectResult("Something went wrong");

            return new OkObjectResult(result);
        }
    }
}