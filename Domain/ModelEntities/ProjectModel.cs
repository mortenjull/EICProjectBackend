using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelEntities
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        public DateTime ProjectCreated { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
    }
}
