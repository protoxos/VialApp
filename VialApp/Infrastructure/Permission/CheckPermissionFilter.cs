using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace VialApp.Infrastructure.Permission
{
    public class CheckPermissionFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public CheckPermissionFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claim = context.HttpContext.User.Claims.Where(c => c.Type == _claim.Type).FirstOrDefault();
            if (claim != null)
            {
                string userInternalId = claim.Value;
            }
            else
                context.Result = new ForbidResult();
        }
    }
}
