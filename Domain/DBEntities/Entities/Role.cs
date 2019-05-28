using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities
{
    public class Role : IEntity
    {       
        public const string User = "User";
        public const string Admin = "Admin";
        public int Id { get; set; }
        public DateTime Created { get; set; }

        public string RoleName { get; set; }
    }
}
