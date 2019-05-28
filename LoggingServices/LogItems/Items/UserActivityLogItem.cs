using System;
using System.Collections.Generic;
using System.Text;
using LoggingServices.LogItems.Activities;
using LoggingServices.LogItems.Interfaces;

namespace LoggingServices.LogItems.Items
{
    public class UserActivityLogItem : IUserActivityLog
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public ActivityTypes.Activities Activity { get; set; }
        public ActivityTypes.LogTypes LogType { get; set; }
        public int ImplicatedItemId { get; set; }
        public int UserId { get; set; }     
    }
    
}
