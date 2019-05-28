using Domain.DBEntities.RelationEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities
{
    public class Organisation : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string OrganisationName { get; set; }
        public string Address { get; set; } 
        public int? MainOrganisationId { get; set; }
        public bool IsMainOrganisation { get; set; }
        public bool EICColaboration { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int OrganisationId { get; set; }


    }
}
