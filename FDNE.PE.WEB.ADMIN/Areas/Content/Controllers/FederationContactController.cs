using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDNE.PE.CORE.Helpers;

namespace FDNE.PE.WEB.ADMIN.Areas.Content.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    [RouteArea(ConstantHelpers.ROUTES.AREA_NAME.CONTENT, AreaPrefix = ConstantHelpers.ROUTES.AREA_PREFIX.CONTENT)]
    [RoutePrefix("contacto")]
    public class FederationContactController : Controller
    {
        [Route]
        public ActionResult Index() => View();
    }
}
