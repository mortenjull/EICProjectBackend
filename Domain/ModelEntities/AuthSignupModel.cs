using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.DBEntities;

namespace Domain.ModelEntities
{
    public class AuthSignupModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
