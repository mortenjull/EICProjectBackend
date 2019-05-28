using System;
using System.Collections.Generic;
using System.Text;
using LoggingServices.LogItems.Interfaces;

namespace LoggingServices.LogItems.Activities
{
    public class ActivityTypes
    {
        /// <summary>
        /// List of activities available for log description.
        /// </summary>
        public enum Activities 
        {
            OrganisationCreated,
            OrganisationRead,
            OrganisationReadAll,
            OrganisationUpdated,
            OrganisationDeleted,

            ResearcherCreated,
            ResearcherRead,
            ResearcherReadWithEverything,
            ResearcherReadWithCategories,
            ResearcherReadAll,
            ResearcherReadAllWithEverything,
            ResearcherReadAllWithCategories,
            ResearcherUpdated,
            ResearcherDeleted,

            ProjectCreated,
            ProjectRead,
            ProjectReadAll,
            ProjectUpdated,
            ProjectDeleted,

            ActivityCreated,
            ActivityRead,
            ActivityReadAll,
            ActivityUpdated,
            ActivityDeleted,

            CategoryCreated,
            CategoryRead,
            CategoryReadAll,
            CategoryUpdated,
            CategoryDeleted,

            ActivityAddedToResearcher,
            ActivityOfResearcherReadALL,
            ActivityRemovedFromResearcher,

            TitleCreated,
            TitleRead,
            TitleReadAll,
            TitleUpdated,
            TitleDeleted,

            UserLogin,
            UserCreated,

            LogCreated,

            Error
        }

        /// <summary>
        /// Areas/Classes of the application for easy eroor location
        /// </summary>
        public enum Areas
        {
            Logging,

            AuthSigninHandler,
            AuthSignupHandler,

            CreateActivityResearchHandler,
            DeleteActivityResearchHandler,
            ReadAllActivityResearchHandler,
            CreateActivityHandler,
            DeleteActivityHandler,
            ReadActivityHandler,
            ReadAllActivityHandler,
            UpdateActivityHandler,

            ReadAllCategoriesHandler,

            CreateOrganisationHandler,
            DeleteOrganistionHandler,
            ReadAllOrginsationsHandler,
            ReadOrganisationHandler,
            UpdateOrganisationHandler,

            CreateProjectHandler,
            ReadAllProjectsHandler,
            ReadProjectHandler,
            DeleteProjectHandler,

            CreateResearcherHandler,
            DeleteResearcherHandler,
            ReadAllResearchers,
            ReadAllResearchersWithEverythingHandler,
            ReadAllResearchersWithCategoriesHandler,
            ReadResearcherHandler,
            ReadResearcherWithEverythingHandler,
            ReadResearcherWithCategoriesHandler,
            UpdateResearcherHandler,

            ReadAllTitlesHandler
        }
        /// <summary>
        /// Available log types.
        /// </summary>
        public enum LogTypes
        {
            UserActivity,
            SystemDiagnostic,
            Error
        }



        //Const´s no longer used but being kept in case new solution not working.

        ////Organisation Acticities.
        //public const string OrganisationCreated = "OrganisationCreated";
        //public const string OrganisationUpdated = "OrganisationUpdated";
        //public const string OrganisationDeleted = "OrganisationDeleted";
        ////Researcher Activities.
        //public const string ResearcherCreated = "ResearcherCreated";
        //public const string ResearcherUpdated = "ResearcherUpdated";
        //public const string ResearcherDeleted = "ResearcherDeleted";
        ////Project Activities.
        //public const string ProjectCreated = "ProjectCreated";
        //public const string ProjectUpdated = "ProjectUpdated";
        //public const string ProjectDeleted = "ProjectDeleted";
        ////Activity Activities.
        //public const string ActivityCreated = "ActivityCreated";
        //public const string ActivityUpdated = "ActivityUpdated";
        //public const string ActivityDeleted = "ActivityDeleted";
        ////Login Activities.
        //public const string UserLogin = "UserLoggedIn";
        //public const string UserCreated = "UserCreated";

    }
}
