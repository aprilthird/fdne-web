using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDNE.PE.CORE.Helpers;

namespace FDNE.PE.WEB.ADMIN.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    [RouteArea(ConstantHelpers.ROUTES.AREA_NAME.ADMIN, AreaPrefix = ConstantHelpers.ROUTES.AREA_PREFIX.ADMIN)]
    [RoutePrefix("disciplinas")]
    public class DisciplineController : Controller
    {
        [Route]
        public ActionResult Index() => View();
    }
}
