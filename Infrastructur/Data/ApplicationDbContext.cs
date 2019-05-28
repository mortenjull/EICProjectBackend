using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DBEntities;
using Domain.DBEntities.RelationEntities; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructur.Data
{
    public class ApplicationDbContext : IdentityDbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { 
        } 
    }
}
