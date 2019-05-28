using System;
using System.Collections.Generic;


namespace Domain.DBEntities
{
    public class Project : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string ProjectName { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
