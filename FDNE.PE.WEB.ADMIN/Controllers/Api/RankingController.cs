using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FDNE.PE.WEB.ADMIN.Dtos;
using System.Data.Entity;
using FDNE.PE.DATA.Models;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/rankings")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class RankingController : BaseApiController
    {
        public RankingController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(Guid? disciplineId = null, Guid? seasonId = null, Guid? categoryId = null, Guid? levelId = null)
        {
            var query = _context.Rankings.AsQueryable();
            if(disciplineId.HasValue)
                query = query.Where(x => x.DisciplineId == disciplineId.Value);
            if(seasonId.HasValue)
                query = query.Where(x => x.SeasonId == seasonId.Value);
            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);
            if (levelId.HasValue)
                query = query.Where(x => x.LevelId == levelId.Value);
            var result = await query.Select(x => new RankingDto
            {
                Id = x.Id,
                Name = x.Name,
                DisciplineId = x.DisciplineSeason.DisciplineId,
                CategoryId = x.CategoryId,
                LevelId = x.LevelId,
                Discipline = new DisciplineDto
                {
                    Name = x.DisciplineSeason.Discipline.Name
                },
                Season = new SeasonDto
                {
                    Year = x.DisciplineSeason.Season.Year
                },
                Category = x.CategoryId.HasValue ? new CategoryDto
                {
                    Name = x.Category.Name
                } : null,
                Level = x.LevelId.HasValue
                    ? new LevelDto
                    {
                        Name = x.Level.Name
                    }
                    : null
            }).AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var result = await _context.Rankings
                .Where(x => x.Id == id)
                .Select(x => new RankingDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        SeasonId = x.SeasonId,
                        DisciplineId = x.DisciplineId,
                        CategoryId = x.CategoryId,
                        LevelId = x.LevelId
                    })
                .FirstOrDefaultAsync();
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(RankingDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var season = await _context.Seasons.FindAsync(model.SeasonId);
            if (season == null)
                return BadRequest($"No se pudo encontrar la temporada con Id '{model.SeasonId}'.");
            if (season.IsFinished)
                return BadRequest($"No se puede modificar una temporada finalizada.");
            if (await _context.Rankings.AnyAsync(x => x.DisciplineId == model.DisciplineId
                 && x.SeasonId == model.SeasonId && x.CategoryId == model.CategoryId && x.LevelId == model.LevelId))
                return BadRequest($"Ya existe otro ranking en la misma temporada con los mismos parámetros.");
            var disciplineSeason = await _context.DisciplineSeasons
                .Where(x => x.SeasonId == model.SeasonId &&
                            x.DisciplineId == model.DisciplineId)
                .FirstOrDefaultAsync();
            if (disciplineSeason == null)
            {
                disciplineSeason = new DisciplineSeason()
                {
                    SeasonId = model.SeasonId,
                    DisciplineId = model.DisciplineId
                };
                _context.DisciplineSeasons.Add(disciplineSeason);
                await _context.SaveChangesAsync();
            }
            var tournament = new Ranking()
            {
                Name = model.Name,
                SeasonId = disciplineSeason.SeasonId,
                DisciplineId = disciplineSeason.DisciplineId,
                CategoryId = model.CategoryId,
                LevelId = model.LevelId
            };
            _context.Rankings.Add(tournament);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, RankingDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var season = await _context.Seasons.FindAsync(model.SeasonId);
            if (season == null)
                return BadRequest($"No se pudo encontrar la temporada con Id '{model.SeasonId}'.");
            if (season.IsFinished)
                return BadRequest($"No se puede modificar una temporada finalizada.");
            if (await _context.Rankings.AnyAsync(x => x.Id != model.Id && x.DisciplineId == model.DisciplineId
                 && x.SeasonId == model.SeasonId && x.CategoryId == model.CategoryId && x.LevelId == model.LevelId))
                return BadRequest($"Ya existe otro ranking en la misma temporada con los mismos parámetros.");
            var tournament = await _context.Rankings.FindAsync(id);
            if (tournament.DisciplineId != model.DisciplineId
                || tournament.SeasonId != model.SeasonId)
            {
                var disciplineSeason = await _context.DisciplineSeasons
                    .FirstOrDefaultAsync(x =>
                        x.DisciplineId == model.DisciplineId &&
                        x.SeasonId == model.SeasonId);
                if(disciplineSeason == null)
                {
                    disciplineSeason = new DisciplineSeason()
                    {
                        SeasonId = model.SeasonId,
                        DisciplineId = model.DisciplineId
                    };
                    _context.DisciplineSeasons.Add(disciplineSeason);
                    await _context.SaveChangesAsync();
                }
                tournament.SeasonId = disciplineSeason.SeasonId;
                tournament.DisciplineId = disciplineSeason.DisciplineId;
            }
            tournament.Name = model.Name;
            tournament.CategoryId = model.CategoryId;
            tournament.LevelId = model.LevelId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var tournament = await _context.Rankings.FirstOrDefaultAsync(x => x.Id == id);
            if (tournament == null)
                return BadRequest($"Ranking con Id '{id}' no encontrada.");
            _context.Rankings.Remove(tournament);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
