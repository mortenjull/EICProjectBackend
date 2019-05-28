using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities
{
    public class ResearcherCategory : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Researcher Researcher { get; set; }

        public int ResearcherId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

    }
}
