using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Context;
using FDNE.PE.WEB.ADMIN.Areas.Admin.ViewModels.ResultViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.ADMIN.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    [RouteArea(ConstantHelpers.ROUTES.AREA_NAME.ADMIN, AreaPrefix = ConstantHelpers.ROUTES.AREA_PREFIX.ADMIN)]
    [RoutePrefix("resultados")]
    public class ResultController : Controller
    {
        private FdneContext _context;

        public ResultController()
        {
            _context = new FdneContext();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _context.Dispose();
            }
        }

        [Route]
        public ActionResult Index() => View();

        [Route("evento/{eventId}/ranking/{rankingId}/calificar")]
        public async Task<ActionResult> Scores(Guid eventId, Guid rankingId)
        {
            var disciplineName = ConstantHelpers.DISCIPLINE.VALUES[ConstantHelpers.DISCIPLINE.TRAINING];
            var model = await _context.Rankings
                .Where(x => x.Id == rankingId)
                .Select(x => new RankingEventViewModel
                {
                    Discipline = x.DisciplineSeason.Discipline.Name,
                    Season = x.DisciplineSeason.Season.Year,
                    Category = x.Category.Name,
                    Level = x.Level.Name,
                    IsDouble = x.Discipline.Name == disciplineName
                }).FirstOrDefaultAsync();
            var @event = await _context.Events.FindAsync(eventId);
            model.EventId = @event.Id;
            model.EventName = @event.Name;
            return View(model);
        }
    }
}