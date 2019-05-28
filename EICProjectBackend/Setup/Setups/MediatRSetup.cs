using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EICProjectBackend.Setup.Setups
{
    public class MediatRSetup : ServiceSetup, ISetup
    {
        public MediatRSetup(IServiceCollection services, IConfiguration config) : base(services, config)
        {
        }

        public override void Run()
        {
            // Add MediatR to the Web api. MediatR search the WebApi Assembly for Commands and
            // command handlers. 
            Services.AddMediatR(typeof(Startup));
        }
    }
}
