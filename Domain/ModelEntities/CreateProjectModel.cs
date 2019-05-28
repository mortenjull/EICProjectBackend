using Domain.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelEntities
{
    public class CreateProjectModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ResearcherCategory> ResearcherCategories { get; set; }
        public List<Category> Categories { get; set; }
        public List<Organisation> Organisations { get; set; }
    }
}
