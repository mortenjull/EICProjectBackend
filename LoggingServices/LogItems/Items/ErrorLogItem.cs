using System;
using System.Collections.Generic;
using System.Text;
using LoggingServices.LogItems.Activities;
using LoggingServices.LogItems.Interfaces;

namespace LoggingServices.LogItems.Items
{
    public class ErrorLogItem : IErrorLog
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public ActivityTypes.Activities Activity { get; set; }
        public ActivityTypes.LogTypes LogType { get; set; }
        public ActivityTypes.Areas Area { get; set; }
        public string ErrorMessage { get; set; }
    }
}
