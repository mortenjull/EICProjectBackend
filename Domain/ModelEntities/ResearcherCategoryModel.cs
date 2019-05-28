using Domain.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelEntities
{
    public class ResearcherCategoryModel
    {
        public Researcher Researcher { get; set; }
        public Category Category { get; set; }
    }
}
