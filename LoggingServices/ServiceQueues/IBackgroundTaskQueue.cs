using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoggingServices.LogItems.Interfaces;

namespace LoggingServices.ServiceQueues
{
    public interface IBackgroundTaskQueue<T> where T : ILogItem
    {
        /// <summary>
        /// Adds the given item to the ConcurrentQueue
        /// </summary>
        /// <param name="workItem">Item of type ILogItem</param>
        void QueueBackgroundWorkItem(T workItem);
        /// <summary>
        /// Removes item from concurrentQueue when queue is available.
        /// </summary>
        /// <param name="cancellationToken">Cancelation callback</param>
        /// <returns>Item of type ILogItem</returns>
        Task<T> DequeueAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Checks if the concurrentqueue has any items.
        /// </summary>
        /// <param name="cancellationToken">Cancelation callback</param>
        /// <returns>true or false</returns>
        Task<bool> QueueIsEmpty(CancellationToken cancellationToken);
    }
}
