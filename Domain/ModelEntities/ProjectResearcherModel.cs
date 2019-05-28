using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelEntities
{
    public class ProjectResearcherModel
    {
        public int ReseacherId { get; set; }
        public string ResearcherName { get; set; }

        public int ProjectResearcherCatId { get; set; }
        public string ProjectResearcherCatName { get; set; }
        
        public string StatusName { get; set; }
        public int StatusId { get; set; }
    }
}
