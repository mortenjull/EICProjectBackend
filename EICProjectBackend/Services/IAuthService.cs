using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DBEntities;

namespace EICProjectBackend.Services
{
    public interface IAuthService
    {
        string CreateToken(User user, string role);

        /// <summary>
        /// calcualtes the hasshed value based on input
        /// </summary>
        /// <param name="input">The string to be hashed</param>
        /// <returns></returns>
        string CalculateHash(string input);
        

        /// <summary>
        /// Matches the input with the hashed value.
        /// </summary>
        /// <param name="hash">Hash from the DB</param>
        /// <param name="input">Input from the user.</param>
        /// <returns></returns>
        bool CheckMatch(string hash, string input); 
    }
}
