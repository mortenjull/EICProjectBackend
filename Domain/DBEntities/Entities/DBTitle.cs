using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities.Entities
{
    public class DBTitle : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

        public string Title { get; set; }
    }
}
