using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Context;
using FDNE.PE.DATA.Models;
using FDNE.PE.WEB.ADMIN.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.ADMIN.Areas.Content.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    [RouteArea(ConstantHelpers.ROUTES.AREA_NAME.CONTENT, AreaPrefix = ConstantHelpers.ROUTES.AREA_PREFIX.CONTENT)]
    [RoutePrefix("disciplinas")]
    public class DisciplineController : Controller
    {
        [Route]
        public ActionResult Index() => View();

        [Route("editar/{id}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            var _context = new FdneContext();
            var model = await _context.DisciplinePortals.AsNoTracking().FirstOrDefaultAsync(x => x.DisciplineId == id);
            if (model == null)
            {
                model = new DisciplinePortal()
                {
                    DisciplineId = id,
                    FirstTab = "Primer tab",
                    FirstBody = "Primer Cuerpo",
                    SecondTab = "Segundo tab",
                    SecondBody = "Segundo Cuerpo",
                    ThirdTab = "Tercer tab",
                    ThirdBody = "Tercer Cuerpo",
                    QuarterTab = "Cuarto tab",
                    QuarterBody = "Cuarto Cuerpo",
                    FifthTab = "Quinto tab",
                    FifthBody = "Quinto Cuerpo",
                    SixthTab = "Sexto tab",
                    SixthBody = "Sexto Cuerpo",
                    UrlImage = ""
                };
                _context.DisciplinePortals.Add(model);
                await _context.SaveChangesAsync();
            }
            var result = new DisciplinePortalDto()
            {
                DisciplineId = id,
                FirstTab = model.FirstTab,
                FirstBody = model.FirstBody,
                SecondTab = model.SecondTab,
                SecondBody = model.SecondBody,
                ThirdTab = model.ThirdTab,
                ThirdBody = model.ThirdBody,
                QuarterTab = model.QuarterTab,
                QuarterBody = model.QuarterBody,
                FifthTab = model.FifthTab,
                FifthBody = model.FifthBody,
                SixthTab = model.SixthTab,
                SixthBody = model.SixthBody,
                ImageUrl = model.UrlImage
            };
            return View(result);
        }
    }

}