using Domain.DBEntities;
using Infrastructur.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EICProjectBackend.Setup.Setups
{
    public class IdentitySetup : ServiceSetup, ISetup
    {
        public IdentitySetup(IServiceCollection services, IConfiguration config) : base(services, config)
        {
        }

        public override void Run()
        {
            Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Config.GetConnectionString("DefaultConnection")));         
        }
    }
}
