using Domain.DBEntities.Entities;
using Domain.ModelEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DBEntities
{
    public class Researcher : IEntity
    {
        public int Id { get; set; }

        public string ResearcherName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public bool EICColab { get; set; }
        public DateTime Created { get; set; }
        public List<Category> Categories { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Project> Projects { get; set; }
        public List<Organisation> Organisations { get; set; }
        public List<DBTitle> Titles { get; set; }
    }
}
