using FDNE.PE.DATA.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDNE.PE.DATA.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace FDNE.PE.WEB.ADMIN.Controllers
{
    [HandleError]
    [Authorize]
    public class HomeController : Controller
    {
        private FdneContext _context;
        private ApplicationUserManager _applicationUserManager;
        private RoleManager<IdentityRole> _roleManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _applicationUserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _applicationUserManager = value;
            }
        }


        public HomeController()
        {
            _context = new FdneContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}