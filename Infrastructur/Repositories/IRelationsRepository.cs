using System;
using System.Collections.Generic;
using System.Text;
using Domain.DBEntities;
using Domain.DBEntities.RelationEntities;

namespace Infrastructur.Repositories
{
    public interface IRelationsRepository
    {
        /// <summary>
        /// Creates an OrganisationProject relation.
        /// </summary>
        /// <param name="entity">The Entity to be created.</param>
        /// <returns>The created entity with the given Id.</returns>
        OrganisationProject CreateOrganisationProject(OrganisationProject entity);
        /// <summary>
        /// Deletes and OrganisationProject relation.
        /// </summary>
        /// <param name="id">Uniqe identifier for entity.</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteOrganisationProject(int id);
        /// <summary>
        /// Deletes all OrganisationsProject relation for a given Organisation
        /// </summary>
        /// <param name="id">Organisation id</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteAllOrganisationsProjects(int id);
        /// <summary>
        /// Updates the owner of all OrganisationProjects with pecific owner.
        /// </summary>
        /// <param name="oldId">Former owner</param>
        /// <param name="newId">The new owner</param>
        /// <returns>true or false</returns>
        bool UpdateAllOrganisationsProjectsOrganisationId(int oldId, int newId);
        /// <summary>
        /// Creates an OrganisationResearcher relation.
        /// </summary>
        /// <param name="entity">The Entity to be created.</param>
        /// <returns>The created entity with the given Id.</returns>
        OrganisationResearcher CreateOrganisationResearcher(OrganisationResearcher entity);
        /// <summary>
        /// Deletes and OrganisationResearcher relation.
        /// </summary>
        /// <param name="reseacherID">Uniqe identifier for reasearcher.</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteOrganisationResearcher(int reseacherID);
        /// <summary>
        /// Delete all organisation researchers for a given organisation.
        /// </summary>
        /// <param name="id">organisation id</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteAllOrganisationResearchers(int id);
        /// <summary>
        /// Updates the owner of all rganisationResearchers with pecific owner.
        /// </summary>
        /// <param name="oldId">Former owner</param>
        /// <param name="newId">The new owner</param>
        /// <returns></returns>
        bool UpdateAllAllOrganisationResearchersOrganisationId(int oldId, int newId);
        /// <summary>
        /// Creates an ProjectCategory relation.
        /// </summary>
        /// <param name="entity">The Entity to be created.</param>
        /// <returns>The created entity with the given Id.</returns>
        ProjectCategory CreateProjectCategory(ProjectCategory entity);
        /// <summary>
        /// Deletes and ProjectCategory relation.
        /// </summary>
        /// <param name="id">Uniqe identifier for entity.</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteProjectCategory(int id);

        /// <summary>
        /// Creates an ProjectResearcher relation.
        /// </summary>
        /// <param name="entity">The Entity to be created.</param>
        /// <returns>The created entity with the given Id.</returns>
        ProjectResearcher CreateProjectResearcher(ProjectResearcher entity);
        /// <summary>
        /// Deletes and ProjectResearcher relation.
        /// </summary>
        /// <param name="researcherID">Uniqe identifier for researcher.</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteProjectResearcher(int researcherID);

        /// <summary>
        /// Creates an ResearcherCategory relation.
        /// </summary>
        /// <param name="entity">The Entity to be created.</param>
        /// <returns>The created entity with the given Id.</returns>
        ResearcherCategory CreateResearcherCategory(ResearcherCategory entity);
        /// <summary>
        /// Deletes and ResearcherCategory relation.
        /// </summary>
        /// <param name="id">Uniqe identifier for entity.</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteResearcherCategory(int id);
        /// <summary>
        /// Deletes and ResearcherCategory relation.
        /// </summary>
        /// <param name="researcherId">Uniqe identifier for reseacher.</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteResearcherCategoryViaReseacherID(int researcherId);
        /// <summary>
        /// Delete ProjectOrganisations for a given project
        /// </summary>
        /// <param name="id">Uniqe identifier for entity.</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteProjectOrganisation(int id);
        /// <summary>
        /// Delete ProjectOrganisations for a given project
        /// </summary>
        /// <param name="id">Uniqe identifier for entity.</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteCategoriesProject(int id);
        /// <summary>
        /// Delete ProjectOrganisations for a given project
        /// </summary>
        /// <param name="id">Uniqe identifier for entity.</param>
        /// <returns>True or False based on result.</returns>
        bool DeleteProjectResearcherWithProjectId(int id);
       
    }
}
