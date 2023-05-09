using FDNE.PE.DATA.Context;
using FDNE.PE.DATA.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.ADMIN.Controllers
{
    public class BaseController : Controller
    {
        protected FdneContext _context;
        protected ApplicationUserManager _userManager;

        protected string UserId => User.Identity.GetUserId();

        protected Task<ApplicationUser> UserAsync => _userManager.FindByIdAsync(User.Identity.GetUserId());

        public BaseController()
        {
            _context = new FdneContext();
            var userStore = new UserStore<ApplicationUser>(_context);
            _userManager = new ApplicationUserManager(userStore);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}