using System;
using System.Collections.Generic;
using System.Text;
using LoggingServices.LogItems.Activities;

namespace LoggingServices.LogItems.Interfaces
{
    public interface IErrorLog : ILogItem
    {
        /// <summary>
        /// Describes the ares the error have ocurred.
        /// </summary>
        ActivityTypes.Areas Area { get; set; }
        /// <summary>
        /// The error message given.
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
