using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VialApp.Infrastructure.Permission
{
    public class CheckPermissionAttribute : TypeFilterAttribute
    {
        public CheckPermissionAttribute(string claimType, string claimValue) : base(typeof(CheckPermissionFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }
}
