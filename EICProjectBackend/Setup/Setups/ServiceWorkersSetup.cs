using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggingServices.LogItems.Interfaces;
using LoggingServices.ServiceQueues;
using LoggingServices.ServiceWorkers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EICProjectBackend.Setup.Setups
{
    public class ServiceWorkersSetup : ServiceSetup, ISetup
    {
        public ServiceWorkersSetup(IServiceCollection services, IConfiguration config) : base(services, config)
        {
        }

        public override void Run()
        {              
            Services.AddHostedService<UserActivityLoggingServiceWorker>();
            Services.AddSingleton<IBackgroundTaskQueue<IUserActivityLog>, UserActivityLogQueue>();
            Services.AddSingleton<IBackgroundTaskQueue<IErrorLog>, ErrorLogQueue>();
        }
    }
}
