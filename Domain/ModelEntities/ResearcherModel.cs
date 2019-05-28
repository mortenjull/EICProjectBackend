using Domain.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelEntities
{
    public class ResearcherModel

    {
        public int ResearcherId { get; set; }
        public string ResearcherName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public bool EICColab { get; set; }
        public List<Category> Categories { get; set; }
    }
}
