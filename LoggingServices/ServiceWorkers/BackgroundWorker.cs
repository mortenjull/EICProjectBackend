using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LoggingServices.ServiceWorkers
{
    public abstract class BackgroundWorker : IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _executingTask = ExecuteAsync(this._cancellationToken.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                this._cancellationToken.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                    cancellationToken));
            }
        }

        public virtual void Dispose()
        {
            this._cancellationToken.Cancel();
        }
    }
}
