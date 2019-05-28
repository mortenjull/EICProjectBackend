using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }     
        public string Password { get; set; }   
        public int RoleId { get; set; }

        
    }
}
