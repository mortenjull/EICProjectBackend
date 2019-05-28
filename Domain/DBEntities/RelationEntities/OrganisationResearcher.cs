using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities
{
    public class OrganisationResearcher : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }   
        public int ResearcherId { get; set; }
        public int OrganisationId { get; set; }
        public int? TitleId { get; set; }
    }
}
