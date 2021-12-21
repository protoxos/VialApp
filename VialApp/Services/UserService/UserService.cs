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
        readonly string SQL_GET_USER_BY_INTERNAL_ID = @"
            SELECT
                Id, 
                InternalId, 
                Deleted, 
                Firstname, 
                Lastname, 
                Email, 
                Username, 
                Password,
                Active
            FROM [User] 
            WHERE InternalId = @InternalId";
        readonly string SQL_GET_USER_BY_ID = @"
            SELECT
                Id, 
                InternalId, 
                Deleted, 
                Firstname, 
                Lastname, 
                Email, 
                Username, 
                Password,
                Active
            FROM [User] 
            WHERE Id = @Id";
        readonly string SQL_GET_USER_BY_UNIQUES = @"
            SELECT
                Id
                , InternalId
                , Deleted
    
                , Firstname
                , Lastname
    
                , Email
                , Username
                , [Password]
                , Active
            FROM [User]
            WHERE
                Username = @Username
	            OR Email = @Email
	            OR InternalId = @InternalId";
        readonly string SQL_GET_USER_BY_EMAIL = @"
            SELECT
                Id
                , InternalId
                , Deleted
    
                , Firstname
                , Lastname
    
                , Email
                , Username
                , [Password]
                , Active
            FROM [User]
            WHERE
	            Email = @Email";
        readonly string SQL_INSERT_USER = @"
            INSERT INTO [User](
                Id, 
                InternalId, 
                Deleted, 
                Firstname, 
                Lastname, 
                Email, 
                Username, 
                Password,
                Active
            ) VALUES (
                NEWID(), 
                0, 
                @Firstname, 
                @Lastname, 
                @Email, 
                @Username, 
                @ePass,
                0
            )
            SELECT SCOPE_IDENTITY()
        ";
        readonly string SQL_ACTIVATE_USER = @"
            UPDATE [User] SET Active = 1 WHERE InternalId = @InternalId
            ";
        #endregion
        public UserService()
        {
            SQL_ACTIVATE_USER += SQL_GET_USER_BY_INTERNAL_ID;
        }

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
        public UserModel? Create(UserModel UserInfo)
        {
            // Si no vienen los datos requeridos, terminamos
            if (!validateRequired(UserInfo))
                return null;

            // Si ya existe un usuario con esos datos, terminamos
            UserModel? userRegister = GetUserByUnques(UserInfo);
            if (userRegister != null)
                return null;

            using (var cnn = ConnectionFactory.CreateConnection())
            {
                // Creamos el usuario con lo que venga.
                int newId = cnn
                    .Query<int>( SQL_INSERT_USER, new { UserInfo } )
                    .FirstOrDefault();

                if (newId >= 1)
                    userRegister = GetUser(newId);
            }
            return userRegister;
        }

        #region GetUser
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
        public UserModel GetUser(int Id)
        {
            using(var cnn = ConnectionFactory.CreateConnection())
            {
                UserModel? u = cnn.Query<UserModel>(
                    SQL_GET_USER_BY_ID,
                    new { Id }
                   ).FirstOrDefault();

                return u??new UserModel();
            }
        }
        public UserModel? GetUserByUnques(UserModel userInfo)
        {
            using (var cnn = ConnectionFactory.CreateConnection())
            {
                UserModel? user = cnn.Query<UserModel>(
                    SQL_GET_USER_BY_UNIQUES, 
                    new { userInfo }
                )
                    .FirstOrDefault();
                return user;
            }
        }
        public UserModel? GetUserByEmail(string Email)
        {
            using (var cnn = ConnectionFactory.CreateConnection())
            {
                UserModel? user = cnn.Query<UserModel>(
                    SQL_GET_USER_BY_EMAIL, 
                    new { Email }
                )
                    .FirstOrDefault();
                return user;
            }
        }
        #endregion

        #region Activation Methods
        public bool Activate(string Email, string Hash)
        {
            UserModel? user = GetUserByEmail(Email);

            if (user == null)
                return false;

            if(Hashing.GetSHA256(user.InternalId.ToString()) == Hash)
            {
                using (var cnn = ConnectionFactory.CreateConnection())
                {
                    return cnn
                        .Query<UserModel>(
                            SQL_ACTIVATE_USER,
                            new { user.InternalId }
                        )
                        .Select(s => s.Active)
                        .FirstOrDefault();
                }
            }
            return false;
        }
        public bool SendVerifyEmail(UserModel userModel)
        {
            string hash = Hashing.GetSHA256(userModel.InternalId.ToString());
            return true;
        }
        #endregion
        bool validateRequired(UserModel userModel)
        {
            return
                !string.IsNullOrEmpty(userModel.Email)
                && isValidEmail(userModel.Email)
                && !string.IsNullOrEmpty(userModel.Password)
                && !string.IsNullOrEmpty(userModel.Firstname);
        }
        bool isValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return false; // suggested by @TK-421

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
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
