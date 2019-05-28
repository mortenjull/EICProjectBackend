using System;
using System.Collections.Generic;
using System.Text;
using LoggingServices.LogItems.Activities;

namespace LoggingServices.LogItems.Interfaces
{
    public interface ILogItem
    {
        int Id { get; set; }
        /// <summary>
        /// Log creation.
        /// </summary>
        DateTime TimeStamp { get; set; }
        /// <summary>
        /// The activity ocurred.
        /// </summary>
        ActivityTypes.Activities Activity { get; set; }
        /// <summary>
        /// Logging type.
        /// </summary>
        ActivityTypes.LogTypes LogType { get; set; } 
    }
}
