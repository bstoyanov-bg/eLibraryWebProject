﻿using System.Security.Claims;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdminRole);
        }
    }
}