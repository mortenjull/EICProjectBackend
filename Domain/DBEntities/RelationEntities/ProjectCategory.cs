using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities
{
    public class ProjectCategory : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int ProjectId { get; set; }       
        public int CategoryId { get; set; }       
    }
}
