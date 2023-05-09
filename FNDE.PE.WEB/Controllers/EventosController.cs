using FNDE.PE.WEB.PORTAL.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FNDE.PE.WEB.PORTAL.ViewModels.EventViewModels;
using FDNE.PE.CORE.Helpers;

namespace FDNE.PE.WEB.PORTAL.Controllers
{
    public class EventosController : BaseController
    {
        public EventosController() : base()
        {

        }
        // GET: Eventos

        public ActionResult Detalle()
        {
            return View();
        }

        public ActionResult Listado() => View();

        public async Task<ActionResult> _PartialEvents(int month)
        {
            var currentYear = DateTime.UtcNow.ToDefaultTimeZone().Year;
            var data = await _context.Events
                .Where(x => x.StartTime.Year == currentYear && x.StartTime.Month == month)
                .ToListAsync();
            var model = data.Select(x => new EventoViewModel
                {
                    Id = x.Id,
                    Acronym = x.Acronym,
                    Day = x.StartTime.ToString("dd"),
                    Month = x.StartTime.ToString("MMMM"),
                    Year = x.StartTime.ToString("yyyy")
                } ).ToList();
            return PartialView(model);
        }

        public async Task<ActionResult> GetEvent(Guid eventId)
        {
            var @event = await _context.Events
                .Include(x => x.Club)
                .Where(x => x.Id == eventId)
                .FirstOrDefaultAsync();
            var result =
                new
                {
                    id = @event.Id,
                    name = @event.Name,
                    clubName = @event.Club.Name,
                    clubLat = @event.Club.Latitude == 0 ? -12.0262674 : @event.Club.Latitude,
                    clubLng = @event.Club.Longitude == 0 ? -77.1282085 : @event.Club.Longitude,
                    date = @event.StartTime.ToDefaultTimeZone().Date != @event.EndTime.ToDefaultTimeZone().Date
                            ? (@event.StartTime.ToDefaultTimeZone().Date - @event.EndTime.ToDefaultTimeZone().Date).Days > 1
                                ? $"{@event.StartTime.ToDefaultTimeZone().Day} al {@event.EndTime.ToDefaultTimeZone().Day} {@event.StartTime.ToString("MMM")}"
                                : $"{@event.StartTime.ToDefaultTimeZone().Day} y {@event.EndTime.ToDefaultTimeZone().Day} {@event.StartTime.ToString("MMM")}"
                            : $"{@event.StartTime.ToDefaultTimeZone().Day} {@event.StartTime.ToString("MMM")}"
                };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}