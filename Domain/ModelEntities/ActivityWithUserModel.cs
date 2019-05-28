using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelEntities
{
    public class ActivityWithUserModel
    {
        public int Id { get; set; }
        public string ActivityName { get; set; }
        public string Username { get; set; } 
        public DateTime Created { get; set; }

    }
}
