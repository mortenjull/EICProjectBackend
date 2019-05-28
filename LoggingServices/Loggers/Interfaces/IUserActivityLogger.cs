using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LoggingServices.LogItems.Activities;
using LoggingServices.LogItems.Interfaces;

namespace LoggingServices.Loggers.Interfaces
{
    public interface IUserActivityLogger<T> where T: IUserActivityLog
    {
        /// <summary>
        /// Reads all log elements.
        /// </summary>
        /// <returns>List og LogItems</returns>
        List<T> ReadAll();

        /// <summary>
        /// Reads all log elements for certain user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List og LogItems</returns>
        List<T> ReadAll(int userId);

        /// <summary>
        /// Reads all log elements of certain Activity type.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>List og LogItems</returns>
        List<T> ReadAll(ActivityTypes.Activities activity);

        /// <summary>
        /// Reads all log elements inside certain periode.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>List og LogItems</returns>
        List<T> ReadAll(DateTime from, DateTime to);

        /// <summary>
        /// Creates the given logItem.
        /// </summary>
        /// <param name="logItem"></param>
        /// <returns>Returns empty Task</returns>
        Task<T> LogThisActivity(T item);

        /// <summary>
        /// Deletes specific log item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns empty Task</returns>
        Task<T> DeleteUserActivityLog(int id);

        /// <summary>
        /// Deletes all log items beloging to specific user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns empty Task</returns>
        Task<T> DeleteUserActivityLogs(int userId);
    }
}
