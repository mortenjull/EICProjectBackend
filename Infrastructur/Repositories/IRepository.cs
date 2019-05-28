using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DBEntities;

namespace Infrastructur.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Creates the given Entity
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>The entity with its given id.</returns>
        T Create(T entity);

        /// <summary>
        /// Reads a entity via the given id.
        /// </summary>
        /// <param name="id">Uniqe identifier for the entity.</param>
        /// <returns>The defined entity.</returns>
        Task<T> Read(int id);

        /// <summary>
        /// Reads all entities of the given type.
        /// </summary>
        /// <returns>A list of defined entities.</returns>
        Task<List<T>> ReadAll();

        /// <summary>
        /// Updates a given entity with the same uniqe identifier.
        /// </summary>
        /// <param name="entity">The entity with the new data.</param>
        /// <returns>The updated entity.</returns>
        T Update(T entity);

        /// <summary>
        /// Delete the Entity with the same uniqe identifier.
        /// </summary>
        /// <param name="id">Uniqe identifier.</param>
        /// <returns>True or False based on result.</returns>
        bool Delete(int id);
    }
}
