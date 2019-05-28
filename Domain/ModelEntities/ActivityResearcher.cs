using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelEntities
{
    public class ActivityResearcher
    {
        public int Id { get; set; }
        public int ResearcherId { get; set; } 
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
    }
}
