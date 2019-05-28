using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelEntities
{
    public class ProjectReadModel
    {
        public ProjectModel ProjectModel { get; set; }
        public List<OrganisationModel> OrganisationsModels { get; set; }
        public List<CategoryModel> CategoryModels { get; set; }
        public List<ProjectResearcherModel> ResearcherModels { get; set; }       
    }
}
