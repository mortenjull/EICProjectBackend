using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace EICProjectBackend.Setup.Setups
{
    public class SwaggerSetup : ServiceSetup, ISetup
    {
        public SwaggerSetup(IServiceCollection services, IConfiguration config) : base(services, config)
        {
        }

        public override void Run()
        {
            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "EICProject Backend-API", Version = "v1" });
            });
        }
    }
}
