﻿using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.ADMIN.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    [RouteArea(ConstantHelpers.ROUTES.AREA_NAME.ADMIN, AreaPrefix = ConstantHelpers.ROUTES.AREA_PREFIX.ADMIN)]
    [RoutePrefix("eventos")]
    public class EventController : Controller
    {
        [Route]
        public ActionResult Index() => View();
    }
}