using FNDE.PE.WEB.PORTAL.Controllers;
using FNDE.PE.WEB.PORTAL.ViewModels.AfiliadosViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.PORTAL.Controllers
{
    [RoutePrefix("afiliados")]
    public class AfiliadosController : BaseController
    {
        [Route("clubes")]
        // GET: Afiliados
        public async Task<ActionResult> ListadoClubes()
        {
            var data = await _context.Clubs.ToListAsync();
            var model = data.Select(x => new ClubViewModel
            {
                Id = x.Id,
                Name = x.Name,
                urlPicture = string.IsNullOrEmpty(x.UrlPicture) 
                    ? Url.Content("~/Content/src/img/club-sample.jpg") : ToImageStorageUrl(x.UrlPicture)
            }).ToList();
            return View(model);
        }

        [Route("listadojinetesapi")]
        public async Task<JsonResult> ListadoJinetesApi()
        {
            var data = await _context.Riders.Include(x => x.User)
                .Include(x => x.RiderClubs.Select(r => r.Club))
                .ToListAsync();
            var model = data.Select(x => new
            {
                Name = String.Format("{0} {1}, {2}", x.User.PaternalSurname, x.User.MaternalSurname, x.User.Name),
                Edad = !x.User.BirthDate.HasValue ? "-" : ((DateTime.UtcNow - x.User.BirthDate.Value).TotalDays / 365).ToString(),
                Categoria = "-",
                Club = string.Join(",", x.RiderClubs.Select(r => r.Club.Name))
            }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Route("listadocaballosapi")]
        public async Task<JsonResult> ListadoCaballosApi()
        {
            var data = await _context.Horses.Include( x => x.HorseClubs.Select(y => y.Club))
                .ToListAsync();
            var model = data.Select(x => new
            {
                Caballo = x.Name,
                Propio = x.BelongsToUs == true ? "Si" : "No",
                Sexo = x.Sex == 0 ? "Hembra" : "Macho",
                Club = string.Join(",", x.HorseClubs.Select(h => h.Club.Name))
            }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Route("detalleclub")]
        public ActionResult DetalleClub()
        {
            return View();
        }
        [Route("jinetes")]
        public ActionResult ListadoJinetes()
        {
            return View();
        }
        [Route("caballos")]
        public ActionResult ListadoCaballos()
        {
            return View();
        }
        [Route("coaches")]
        public async Task<ActionResult> ListadoCoaches()
        {
            var model = await _context.CoachingSystems.
                Select(x => new CoachingViewModel
                {
                    id = x.Id,
                    firstTab = x.FirstTab,
                    firstBody = x.FirstBody,
                    secondTab = x.SecondTab,
                    secondBody = x.SecondBody,
                    thirdTab = x.ThirdTab,
                    thirdBody = x.ThirdBody,
                    quarterTab = x.QuarterTab,
                    quarterBody = x.QuarterBody,
                    fifthTab = x.FifthTab,
                    fifthBody = x.FifthBody,
                    sixthTab = x.SixthTab,
                    sixthBody = x.SixthBody,
                }).FirstOrDefaultAsync();
            return View(model);
        }


        [Route("listadocoaching")]
        public async Task<ActionResult> ListadoCoaching()
        {
            var model = await _context.CoachingSystems.
                Select(x => new CoachingViewModel
            {
                id = x.Id,
                firstTab = x.FirstTab,
                firstBody = x.FirstBody,
                secondTab = x.SecondTab,
                secondBody = x.SecondBody,
                thirdTab = x.ThirdTab,
                thirdBody = x.ThirdBody,
                quarterTab = x.QuarterTab,
                quarterBody = x.QuarterBody,
                fifthTab = x.FifthTab,
                fifthBody = x.FifthBody,
                sixthTab = x.SixthTab,
                sixthBody = x.SixthBody,
            }).FirstOrDefaultAsync();
            return View(model);
        }
        [Route("lidea")]
        public ActionResult LIDEA()
        {
            return View();
        }
        [Route("afilicacion")]
        public ActionResult Afiliacion()
        {
            return View();
        }
        [Route("afiliacion-registrar")]
        public ActionResult RegistrarAfiliacion()
        {
            return View();
        }
    }
}