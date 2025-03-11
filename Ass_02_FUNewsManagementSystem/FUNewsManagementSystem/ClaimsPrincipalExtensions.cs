using System;
using System.Security.Claims;

namespace FUNewsManagementSystem
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserRole(this ClaimsPrincipal user)
        {
            var roleClaim = user.FindFirst(ClaimTypes.Role);
            return (roleClaim == null || string.IsNullOrEmpty(roleClaim.Value))
                ? 0
                : int.TryParse(roleClaim.Value, out var parsedRole) ? parsedRole : 0;
        }
    }
}
