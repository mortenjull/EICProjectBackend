using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingServices.LogItems.Interfaces
{
    public interface IUserActivityLog : ILogItem
    {
        /// <summary>
        /// The item involed in the given activity.
        /// </summary>
        int ImplicatedItemId { get; set; }
        /// <summary>
        /// The user involved in the activity.
        /// </summary>
        int UserId { get; set; }
    }
}
