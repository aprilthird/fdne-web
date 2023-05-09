using FDNE.PE.WEB.ADMIN.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Models;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    [RoutePrefix("api/eventos")]
    public class EventController : BaseApiController
    {
        public EventController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(DateTime? start = null, DateTime? end = null, Guid? disciplineId = null, Guid? seasonId = null, Guid? clubId = null, bool? past = null, string startTime = null, string endTime = null)
        {
            var query = _context.Events
                .Include(x => x.Club)
                .Include(x => x.DisciplineSeason.Season)
                .Include(x => x.DisciplineSeason.Discipline)
                .AsQueryable();
            if (disciplineId.HasValue)
                query = query.Where(x => x.DisciplineId == disciplineId.Value);
            if (seasonId.HasValue)
                query = query.Where(x => x.SeasonId == seasonId.Value);
            if (clubId.HasValue)
                query = query.Where(x => x.ClubId == clubId.Value);
            if (past.HasValue && past.Value)
            {
                var utcNow = DateTime.UtcNow;
                query = query.Where(x => x.StartTime <= utcNow).OrderByDescending(x => x.StartTime).AsQueryable();
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                var startDate = startTime.ToUtcDateTime();
                query = query.Where(x => x.StartTime >= startDate);
            }
            if(!string.IsNullOrEmpty(endTime))
            {
                var endDate = endTime.ToUtcDateTime();
                query = query.Where(x => x.EndTime <= endDate);
            }
            if(start.HasValue)
            {
                var utcStart = start.Value.ToUniversalTime();
                query = query.Where(x => x.StartTime.Date >= utcStart.Date);
            }
            if(end.HasValue)
            {
                var utcEnd = end.Value.ToUniversalTime();
                query = query.Where(x => x.EndTime.Date <= utcEnd.Date);
            }
            var data = await query.AsNoTracking().ToListAsync();
            var result = data
                .Select(x => new EventDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Acronym = x.Acronym,
                    DisciplineId = x.DisciplineId,
                    Discipline = new DisciplineDto
                    {
                        Name = x.DisciplineSeason.Discipline.Name
                    },
                    SeasonId = x.SeasonId,
                    Season = new SeasonDto
                    {
                        Year = x.DisciplineSeason.Season.Year
                    },
                    ClubId = x.ClubId,
                    Club = new ClubDto
                    {
                        Name = x.Club.Name
                    },
                    StartTime = x.StartTime.ToLocalDateFormat(),
                    EndTime = x.EndTime.ToLocalDateFormat(),
                    Editable = !x.DisciplineSeason.Season.IsFinished
                        && !x.HasResults
                }).ToList();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Events.FindAsync(id);
            var result = new EventDto
            {
                Id = data.Id,
                Name = data.Name,
                Acronym = data.Acronym,
                ClubId = data.ClubId,
                StartTime = data.StartTime.ToLocalDateFormat(),
                EndTime = data.EndTime.ToLocalDateFormat(),
                DisciplineId = data.DisciplineId,
                SeasonId = data.SeasonId
            };
            return Ok(result);
        }

        [HttpGet, Route("{id}/dias")]
        public async Task<IHttpActionResult> GetDays(Guid id)
        {
            var data = await _context.Events.FindAsync(id);
            return Ok(Enumerable.Range(1,
                (data.EndTime - data.StartTime).Days + 1).Select(x => new
                {
                    dayNumber = x,
                    date = data.StartTime.AddDays(x).ToLocalDateFormat()
                }));
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(EventDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);                                                                           
            if (model.StartTime.ToUtcDateTime() > model.EndTime.ToUtcDateTime())
                return BadRequest("La fecha de inicio no puede mayor a la fecha de fin.");
            var season = await _context.Seasons.FindAsync(model.SeasonId);
            if (season == null)
                return BadRequest($"No se pudo encontrar la temporada con Id '{model.SeasonId}'.");
            if (season.IsFinished)
                return BadRequest("No se puede agregar eventos a una temporada finalizada.");
            var events = await _context.Events.ToListAsync();
            if (events.Any(e => GeneralHelpers.DateTimeConflictClosed(model.StartTime.ToUtcDateTime(), model.EndTime.ToUtcDateTime(), e.StartTime, e.EndTime)))
                return BadRequest("Las fechas seleccionadas coinciden con el de otro evento.");
            var disciplineSeason = await _context.DisciplineSeasons
                .Where(x => x.SeasonId == model.SeasonId && x.DisciplineId == model.DisciplineId)
                .FirstOrDefaultAsync();
            if(disciplineSeason == null)
            {
                disciplineSeason = new DisciplineSeason
                {
                    DisciplineId = model.DisciplineId,
                    SeasonId = model.SeasonId
                };
                _context.DisciplineSeasons.Add(disciplineSeason);
                await _context.SaveChangesAsync();
            }
            var @event = new Event()
            {
                Name = model.Name,
                Acronym = model.Acronym,
                StartTime = model.StartTime.ToUtcDateTime(),
                EndTime = model.EndTime.ToUtcDateTime(),
                HasTwoDates = model.StartTime.ToUtcDateTime() != model.EndTime.ToUtcDateTime(),
                DisciplineId = disciplineSeason.DisciplineId,
                ClubId = model.ClubId,
                SeasonId = disciplineSeason.SeasonId
            };
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, EventDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (model.StartTime.ToUtcDateTime() > model.EndTime.ToUtcDateTime())
                return BadRequest("La fecha de inicio no puede mayor a la fecha de fin.");
            var season = await _context.Seasons.FindAsync(model.SeasonId);
            if (season == null)
                return BadRequest($"No se pudo encontrar la temporada con Id '{model.SeasonId}'.");
            if (season.IsFinished)
                return BadRequest("No se puede agregar eventos a una temporada finalizada.");
            var events = await _context.Events.Where(x => x.Id != model.Id).ToListAsync();
            if (events.Any(e => GeneralHelpers.DateTimeConflictClosed(model.StartTime.ToUtcDateTime(), model.EndTime.ToUtcDateTime(), e.StartTime, e.EndTime)))
                return BadRequest("Las fechas seleccionadas coinciden con el de otro evento.");
            var @event = await _context.Events.FindAsync(id);
            if (@event.DisciplineId != model.DisciplineId
                || @event.SeasonId != model.SeasonId)
            {
                var disciplineSeason = await _context.DisciplineSeasons
                    .Where(x => x.SeasonId == model.SeasonId && x.DisciplineId == model.DisciplineId)
                    .FirstOrDefaultAsync();
                if (disciplineSeason == null)
                {
                    disciplineSeason = new DisciplineSeason
                    {
                        DisciplineId = model.DisciplineId,
                        SeasonId = model.SeasonId
                    };
                    _context.DisciplineSeasons.Add(disciplineSeason);
                    await _context.SaveChangesAsync();
                }
                @event.DisciplineId = disciplineSeason.DisciplineId;
                @event.SeasonId = disciplineSeason.SeasonId;
            }
            @event.Name = model.Name;
            @event.Acronym = model.Acronym;
            @event.StartTime = model.StartTime.ToUtcDateTime();
            @event.EndTime = model.EndTime.ToUtcDateTime();
            @event.HasTwoDates = @event.StartTime != @event.EndTime;
            @event.ClubId = model.ClubId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var @event = await _context.Events
                .Include(x => x.DisciplineSeason.Season)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (@event == null)
                return BadRequest(
                    $"Evento con Id '{id}' no encontrado.");
            if (@event.DisciplineSeason.Season.IsFinished)
                return BadRequest("No se puede modificar un binomio en una temporada finalizada");
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
