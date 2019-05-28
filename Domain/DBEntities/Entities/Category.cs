using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities
{
    public class Category : IEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; } 
        public DateTime Created { get; set; }
        public string CategoryName { get; set; }       
    }
}
