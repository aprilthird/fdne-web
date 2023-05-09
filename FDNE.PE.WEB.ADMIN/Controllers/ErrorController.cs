    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.ADMIN.Controllers
{
    [RoutePrefix("error")]
    public class ErrorController : Controller
    {
        // GET: Error
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("404")]
        public ActionResult PageNotFound()
        {
            return View();
        }
        [Route("500")]
        public ActionResult InternalServerError()
        {
            return View();
        }
    }
}