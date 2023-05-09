using FDNE.PE.DATA.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FNDE.PE.WEB.PORTAL.Extensions
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