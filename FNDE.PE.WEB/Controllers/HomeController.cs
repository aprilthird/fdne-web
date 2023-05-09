using FDNE.PE.CORE.Helpers;
using FNDE.PE.WEB.PORTAL.Controllers;
using FNDE.PE.WEB.PORTAL.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.PORTAL.Controllers
{
    
    public class HomeController : BaseController
    {
        public HomeController() : base()
        {
        }
        
        public async Task<ActionResult> Index()
        {
            var utcNow = DateTime.UtcNow;
            var season = await _context.Seasons.Where(x => x.StartDate <= utcNow && utcNow <= x.EndDate).FirstOrDefaultAsync();
            var disciplines = await _context.Disciplines.ToListAsync();
            var events = season != null ? await _context.Events
                .Where(x => x.SeasonId == season.Id)
                .Include(x => x.Club)
                .OrderBy(x => x.StartTime)
                .ToListAsync() : null;
            var model = new HomeViewModel
            {
                Disciplines = disciplines
                    .Select(x => new DisciplineViewModel
                    {
                        Name = x.Name,
                        Description = x.Description
                    }).ToList(),
                Events = events != null 
                    ? events
                    .Select(x => new EventViewModel
                    {
                        Acronym = x.Acronym,
                        Club = x.Club.Name,
                        Date = x.StartTime.ToDefaultTimeZone().Date != x.EndTime.ToDefaultTimeZone().Date
                            ? (x.StartTime.ToDefaultTimeZone().Date - x.EndTime.ToDefaultTimeZone().Date).Days > 1
                                ? $"{x.StartTime.ToDefaultTimeZone().Day} al {x.EndTime.ToDefaultTimeZone().Day} {x.StartTime.ToString("MMM")}"
                                : $"{x.StartTime.ToDefaultTimeZone().Day} y {x.EndTime.ToDefaultTimeZone().Day} {x.StartTime.ToString("MMM")}"
                            : $"{x.StartTime.ToDefaultTimeZone().Day} {x.StartTime.ToString("MMM")}"
                    }).ToList() : null
            };
            return View(model);
        }

        public ActionResult Test() => View();

        public ActionResult Layout() => View();
    }
}