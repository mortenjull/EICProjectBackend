using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DBEntities;
using Domain.ModelEntities;

namespace Infrastructur.Repositories
{
    public interface IOrganisationRepository : IRepository<Organisation>
    {
        Task<OrganisationWithChildren> ReadOrganisationWithChildren(int id);
        /// <summary>
        /// Resets all organisations with given Id parameter as MainOrganisationId.
        /// Used incase of Main organisation deletion.
        /// </summary>
        /// <param name="MainOrganisationId">Id of deleted MainOrganisation</param>
        /// <returns>True or false</returns>
        Task<bool> ResetMainOrganisationId(int MainOrganisationId);
    }
}
