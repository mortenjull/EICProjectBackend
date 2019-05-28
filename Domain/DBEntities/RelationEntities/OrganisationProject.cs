using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities.RelationEntities
{
    public class OrganisationProject : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int ProjectId { get; set; }       
        public int OrganisationId { get; set; }        
    }
}
