using FDNE.PE.CORE.Helpers;
using FDNE.PE.WEB.ADMIN.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/resultados")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class ResultController : BaseApiController
    {
        public ResultController() : base()
        {
        }

        [HttpGet, Route]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _context.Results
                .Select(x => new ResultDto
                {
                    Score = x.Score,
                    HorseId = x.HorseId,
                    Horse = new HorseDto
                    {
                        Name = x.Horse.Name
                    },
                    RiderId = x.RiderId,
                    Rider = new RiderDto
                    {
                        User = new ApplicationUserDto
                        {
                            Name = x.Rider.User.Name,
                            PaternalSurname = x.Rider.User.PaternalSurname,
                            MaternalSurname = x.Rider.User.MaternalSurname
                        }
                    }
                })
                .AsNoTracking()
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet, Route("~/api/eventos/{eventId}/ranking/{rankingId}/dia/{day}/resultados")]
        public async Task<IHttpActionResult> GetAll(Guid eventId, Guid rankingId, int day)
        {
            var @event = await _context.Events.FindAsync(eventId);
            var date = @event.StartTime.AddDays(day - 1);
            var existsResult = await _context.Results
                .Where(x => x.EventId == eventId && x.RankingId == rankingId && x.Date == date)
                .AnyAsync();    
            var result = existsResult
                ? await _context.Results
                .Where(x => x.EventId == eventId && x.RankingId == rankingId && x.Date == date)
                .OrderByDescending(x => x.Percent)
                .OrderByDescending(x => x.Score)
                .Select(x => new ResultDto
                {
                    Score = x.Score,
                    Percent = x.Percent,
                    HorseId = x.HorseId,
                    ClubId = x.ClubId,
                    Club = new ClubDto
                    {
                        Name = x.Club.Name
                    },
                    Horse = new HorseDto
                    {
                        Name = x.Horse.Name
                    },
                    RiderId = x.RiderId,
                    Rider = new RiderDto
                    {
                        User = new ApplicationUserDto
                        {
                            Name = x.Rider.User.Name,
                            PaternalSurname = x.Rider.User.PaternalSurname,
                            MaternalSurname = x.Rider.User.MaternalSurname
                        }
                    }
                }).AsNoTracking().ToListAsync()
                : await _context.Binomials
                .Where(x => x.RankingId == rankingId)
                .Select(x => new ResultDto
                {
                    Score = 0,
                    Percent = 0,
                    HorseId = x.HorseId,
                    ClubId = x.ClubId,
                    Club = new ClubDto
                    {
                        Name = x.Club.Name
                    },
                    Horse = new HorseDto
                    {
                        Name = x.Horse.Name
                    },
                    RiderId = x.RiderId,
                    Rider = new RiderDto
                    {
                        User = new ApplicationUserDto
                        {
                            Name = x.Rider.User.Name,
                            PaternalSurname = x.Rider.User.PaternalSurname,
                            MaternalSurname = x.Rider.User.MaternalSurname
                        }
                    }
                }).AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpPost, Route("~/api/evento/{eventId}/ranking/{rankingId}/resultados/dia/{day}/calificar")]
        public async Task<IHttpActionResult> CreateRange(Guid eventId, Guid rankingId, int day, IEnumerable<ResultDto> model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ranking = await _context.Rankings
                .Where(x => x.Id == rankingId)
                .Include(x => x.DisciplineSeason.Season)
                .FirstOrDefaultAsync();
            var @event = await _context.Events
                .Where(x => x.Id == eventId)
                .Include(x => x.DisciplineSeason.Season)
                .FirstOrDefaultAsync();
            if (ranking.DisciplineSeason != @event.DisciplineSeason)
                return BadRequest("No se puede registrar la calificación para el ranking en dicho Evento.");
            if (ranking.Season.IsFinished)
                return BadRequest("No se puede registrar una calificación para un período pasado.");
            var date = @event.StartTime.ToDefaultTimeZone().AddDays(day - 1).ToUniversalTime();
            if (date > @event.EndTime)
                return BadRequest($"No existe un día {day} en el evento.");
            var results = await _context.Results
                .Where(x => x.RankingId == rankingId && x.EventId == eventId && x.Date == date)
                .ToListAsync();
            if (!results.Any())
            {
                _context.Results.AddRange(model.Select(x => new DATA.Models.Result
                {
                    EventId = eventId,
                    RankingId = rankingId,
                    HorseId = x.HorseId,
                    RiderId = x.RiderId,
                    ClubId = x.ClubId,
                    Date = date,
                    Score = x.Score,
                    Percent = x.Percent,
                    Plays = true
                }));
            }
            else
            {
                results.ForEach(x => {
                    var r = model.Where(m => m.RiderId == x.RiderId && m.HorseId == x.HorseId && m.ClubId == x.ClubId).FirstOrDefault();
                    x.Score = r.Score;
                    x.Percent = r.Percent;
                });
            }
            @event.HasResults = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
