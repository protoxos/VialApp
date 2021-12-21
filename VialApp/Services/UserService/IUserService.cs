using VialApp.Models;

namespace VialApp.Services
{
    public interface IUserService
    {
        string Auth(AuthRequest auth);
        UserModel GetUser(System.Security.Claims.ClaimsPrincipal claimsPrincipal);
        UserModel GetUser(string UserInternalId);
    }
}
