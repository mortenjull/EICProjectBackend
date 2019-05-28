using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities.Entities
{
    public class Activity : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string ActivityName { get; set; }
    }
}
