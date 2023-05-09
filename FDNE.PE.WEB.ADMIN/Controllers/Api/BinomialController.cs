using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Models;
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
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    [RoutePrefix("api/binomios")]
    public class BinomialController : BaseApiController
    {
        public BinomialController() : base()
        {
        }
        
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _context.Binomials
                .Select(x => new BinomialDto
                {
                    RankingId = x.RankingId,
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
                            MaternalSurname = x.Rider.User.MaternalSurname,
                            PaternalSurname = x.Rider.User.PaternalSurname
                        }
                    }
                })
                .AsNoTracking()
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet, Route("~/api/rankings/{rankingId}/binomios")]
        public async Task<IHttpActionResult> GetByTournament(Guid rankingId)
        {
            var result = await _context.Binomials
                .Where(x => x.RankingId == rankingId)
                .Include(x => x.Rider.User)
                .Select(x => new BinomialDto
                {
                    RankingId = x.RankingId,
                    HorseId = x.HorseId,
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
                            MaternalSurname = x.Rider.User.MaternalSurname,
                            PaternalSurname = x.Rider.User.PaternalSurname
                        }
                    }
                })
                .AsNoTracking()
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Binomials.FindAsync(id);
            var result = new BinomialDto
            {
                RankingId = data.RankingId,
                HorseId = data.HorseId,
                RiderId = data.RiderId,
                ClubId = data.ClubId
            };
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(BinomialDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ranking = await _context.Rankings
                .Include(x => x.DisciplineSeason.Season)
                .FirstOrDefaultAsync(x => x.Id == model.RankingId);
            if (ranking.DisciplineSeason.Season.IsFinished)
                return BadRequest("No se puede agregar un binomio para una temporada finalizada.");
            if (await _context.Binomials.AnyAsync(x => x.RankingId == model.RankingId
                 && x.RiderId == model.RiderId && x.HorseId == model.HorseId))
                return BadRequest("Ya existe dicho binomio dentro del ranking.");
            var binomial = new Binomial()
            {
                HorseId = model.HorseId,
                RiderId = model.RiderId,
                RankingId = model.RankingId,
                ClubId = model.ClubId
            };
            _context.Binomials.Add(binomial);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{rankingId}/{horseId}/{riderId}/{clubId}")]
        public async Task<IHttpActionResult> Delete(Guid rankingId, Guid horseId, string riderId, Guid clubId)
        {
            var binomial = await _context.Binomials
                .Include(x => x.Ranking.DisciplineSeason.Season)
                .FirstOrDefaultAsync(x => x.RankingId == rankingId && x.HorseId == horseId && x.RiderId == riderId && x.ClubId == clubId);
            if (binomial == null)
                return BadRequest(
                    $"Binomio con Id de Ranking '{rankingId}', Caballo '{horseId}', Jinete '{riderId} y Club '{clubId}' no encontrado.");
            if (binomial.Ranking.DisciplineSeason.Season.IsFinished)
                return BadRequest("No se puede modificar un binomio en una temporada finalizada");
            _context.Binomials.Remove(binomial);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
