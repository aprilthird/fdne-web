using FNDE.PE.WEB.PORTAL.Controllers;
using FNDE.PE.WEB.PORTAL.ViewModels.DisciplinaViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.PORTAL.Controllers
{
    [RoutePrefix("disciplina")]
    public class DisciplinasController : BaseController
    {
        public DisciplinasController() : base()
        {
        }

        [Route("{name}")]
        public async Task<ActionResult> Discipline(string name)
        {
            var disciplines = await _context.Disciplines.ToListAsync();
            var discipline = disciplines.FirstOrDefault(x => x.Name.Replace(" ", string.Empty).Normalize() == name.Normalize());
            var disciplineView = await _context.DisciplinePortals
                .Where(x => x.DisciplineId == discipline.Id)
                .FirstOrDefaultAsync();
            var model = disciplineView != null ? new DisciplineViewModel
            {
                Name = discipline.Name,
                Description = discipline.Description,
                Lessons = disciplineView.FirstBody,
                Statutes = disciplineView.SecondBody
            } : new DisciplineViewModel
            {
                Description = "",
                Lessons = "",
                Statutes = ""
            };
            return View(model);
        }
    }
}