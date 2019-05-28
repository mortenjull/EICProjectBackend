
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWork;

namespace EICProjectBackend.Setup.Setups
{
    public class ApplicationSetup : ServiceSetup, ISetup
    {
        public ApplicationSetup(IServiceCollection services, IConfiguration config) : base(services, config)
        {
        }

        public override void Run()
        {
            Services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}
