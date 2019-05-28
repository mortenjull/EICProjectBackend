using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EICProjectBackend.Setup
{
    public abstract class ServiceSetup
        : ISetup
    {
        protected readonly IServiceCollection Services;
        protected readonly IConfiguration Config;

        protected ServiceSetup(IServiceCollection services, IConfiguration config)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            this.Services = services;
            this.Config = config;
        }

        public abstract void Run();
    }
}
