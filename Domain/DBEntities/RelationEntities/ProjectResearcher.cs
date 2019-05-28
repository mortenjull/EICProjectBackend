using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities.RelationEntities
{
    public class ProjectResearcher : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int ProjectId { get; set; }        
        public int ResearcherId { get; set; }
        public int CategoryId { get; set; }
        public int TitleId { get; set; }
    }
}
