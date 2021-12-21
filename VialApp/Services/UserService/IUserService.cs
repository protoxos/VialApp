using VialApp.Models;

namespace VialApp.Services
{
    public interface IUserService
    {
        string Auth(AuthRequest auth);
        UserModel GetUser(System.Security.Claims.ClaimsPrincipal claimsPrincipal);
        UserModel GetUser(string userInternalId);
        UserModel GetUser(int id);
        UserModel? Create(UserModel userModel);
        UserModel? GetUserByUnques(UserModel userModel);
        UserModel? GetUserByEmail(string email);
        bool SendVerifyEmail(UserModel userModel);
        public bool Activate(string Email, string Hash);
    }
}
