using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VialApp.Models;
using VialApp.Tools;

namespace VialApp.Services
{
    public class UserService : IUserService
    {
        #region Constants
        private readonly string SQL_GET_USER_BY_INTERNAL_ID = @"
            SELECT
                Id, 
                InternalId, 
                Deleted, 
                Firstname, 
                Lastname, 
                Email, 
                Username, 
                Password
            FROM [User] 
            WHERE InternalId = @InternalId";
        #endregion

        public string Auth(AuthRequest auth)
        {
            using (var cnn = ConnectionFactory.CreateConnection())
            {
                string ePass = Hashing.GetSHA256(auth.Password);
                string? userGuid = cnn.Query<Guid>(
                    "SELECT InternalId FROM [User] WHERE (Username = @Username OR Email = @Username) AND [Password] = @ePass"
                    , new { auth.Username, ePass })
                    .FirstOrDefault()
                    .ToString();


                return !string.IsNullOrEmpty(userGuid) ? generateToken(userGuid) : "";
            }
        }
        public UserModel GetUser(ClaimsPrincipal Claims)
        {
            var userId = Claims.Claims.Where(c => c.Type == "UserId").FirstOrDefault();
            return GetUser(userId == null ? "" : userId.Value);
        }

        public UserModel GetUser(string UserInternalId)
        {
            using(var cnn = ConnectionFactory.CreateConnection())
            {
                UserModel? u = cnn.Query<UserModel>(
                    SQL_GET_USER_BY_INTERNAL_ID,
                    new { InternalId = UserInternalId }
                   ).FirstOrDefault();

                return u??new UserModel();
            }
        }

        private string generateToken(string UserId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("UserId", UserId.ToString())
                }),
                Expires = null,
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(Configuration.GetSalt()), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
