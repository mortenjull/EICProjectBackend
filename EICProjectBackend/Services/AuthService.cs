using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Domain.DBEntities;
using EICProjectBackend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Infrastructur.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using UnitOfWork;

namespace Services
{
    public class AuthService : IAuthService
    {      
        private readonly IConfiguration _config;
        private readonly IUserRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        private const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
        private const int SALT_LENGTH = 16;

        public AuthService(IConfiguration config, IUnitOfWork unitOfWork)
        {           
            this._config = config;
            this._unitOfWork = unitOfWork;
            this._userRepository = this._unitOfWork.UserRepository;
        }
        
        public string CreateToken(User user, string role)
        {
            //generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._config["Token:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role),
                    //new Claim(ClaimTypes.Country, country.CountryName),
                    new Claim(ClaimTypes.GivenName, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Int32.Parse(this._config["Token:Expire"])),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var writtenToken = tokenHandler.WriteToken(token);

            return writtenToken;
        }

        
        public string CalculateHash(string input)
        {
            var salt = GenerateSalt(SALT_LENGTH);

            var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

            return $"{ Convert.ToBase64String(salt) }:{ Convert.ToBase64String(bytes) }";
        }

        /// <summary>
        /// generated salt. Unique identifier for the hash
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static byte[] GenerateSalt(int length)
        {
            var salt = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            return salt;
        }

        
        public bool CheckMatch(string hash, string input)
        {
            try
            {
                var parts = hash.Split(':');

                var salt = Convert.FromBase64String(parts[0]);

                var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

                return parts[1].Equals(Convert.ToBase64String(bytes));
            }
            catch
            {
                return false;
            }
        }
    }
}
