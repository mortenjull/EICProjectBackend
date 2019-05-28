using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoggingServices.LogItems.Interfaces;

namespace LoggingServices.ServiceQueues
{
    public class ErrorLogQueue : IBackgroundTaskQueue<IErrorLog>
    {
        private ConcurrentQueue<IErrorLog> _workItems = new ConcurrentQueue<IErrorLog>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void QueueBackgroundWorkItem(IErrorLog workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<IErrorLog> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }

        public async Task<bool> QueueIsEmpty(CancellationToken cancellationToken)
        {
            return this._workItems.IsEmpty;
        }
    }
}
