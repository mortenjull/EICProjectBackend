using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DBEntities;

namespace Infrastructur.Repositories
{
    public interface IUserRepository<T> : IRepository<T> where T : IEntity
    {
        Task<T> ReadViaUserName(string username);

        Task<Role> ReadUserRole(int roleId);
    }
}
