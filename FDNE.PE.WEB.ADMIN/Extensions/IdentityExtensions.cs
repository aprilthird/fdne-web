using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using FDNE.PE.DATA.Models;
using Microsoft.AspNet.Identity;

namespace FDNE.PE.WEB.ADMIN.Extensions
{
    public static class IdentityExtensions
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this ApplicationUser user, UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}