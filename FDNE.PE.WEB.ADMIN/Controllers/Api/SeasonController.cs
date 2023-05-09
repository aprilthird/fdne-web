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
using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Models;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/temporadas")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class SeasonController : BaseApiController
    {
        public SeasonController() : base()
        {    
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var data = await _context.Seasons.AsNoTracking().ToListAsync();
            var result = data.Select(x => new SeasonDto
                {
                    Id = x.Id,
                    Year = x.Year,
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate.ToLocalDateFormat(),
                    IsActive = x.IsActive,
                    IsFinished = x.IsFinished
                });
            return Ok(result);
        }

        [HttpGet, Route("habiles")]
        public async Task<IHttpActionResult> GetEnabled()
        {
            var data = await _context.Seasons.AsNoTracking().ToListAsync();
            var result = data
                .Where(x => !x.IsFinished)
                .Select(x => new SeasonDto
                {
                    Id = x.Id,
                    Year = x.Year,
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate.ToLocalDateFormat(),
                    IsActive = x.IsActive,
                    IsFinished = x.IsFinished
                });
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Seasons.FindAsync(id);
            var result = new SeasonDto
            {
                Id = data.Id,
                Year = data.Year,
                StartDate = data.StartDate.ToLocalDateFormat(),
                EndDate = data.EndDate.ToLocalDateFormat()
            };
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(SeasonDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (model.StartDate.ToUtcDateTime() >= model.EndDate.ToUtcDateTime())
                return BadRequest("La fecha de inicio no puede ser mayor o igual a la fecha de fin.");
            var seasons = await _context.Seasons.ToListAsync();
            if (seasons.Any(x => GeneralHelpers.DateTimeConflictOpened(model.StartDate.ToUtcDateTime(), model.EndDate.ToUtcDateTime(), x.StartDate, x.EndDate)))
                return BadRequest("Las fechas seleccionadas coinciden con el de otro período.");
            var season = new Season()
            {
                Year = model.Year,
                StartDate = model.StartDate.ToUtcDateTime(),
                EndDate = model.EndDate.ToUtcDateTime()
            };
            _context.Seasons.Add(season);
            await _context.SaveChangesAsync();
            return Ok(season);
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, SeasonDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var seasons = await _context.Seasons.Where(x => x.Id != id).ToListAsync();
            if (seasons.Any(x => GeneralHelpers.DateTimeConflictOpened(model.StartDate.ToUtcDateTime(), model.EndDate.ToUtcDateTime(), x.StartDate, x.EndDate)))
                return BadRequest("Las fechas seleccionadas coinciden con el de otro período.");
            var season = await _context.Seasons.FindAsync(id);
            season.Year = model.Year;
            season.StartDate = model.StartDate.ToUtcDateTime();
            season.EndDate = model.EndDate.ToUtcDateTime();
            await _context.SaveChangesAsync();
            return Ok(season);
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var season = await _context.Seasons.FirstOrDefaultAsync(x => x.Id == id);
            if (season == null)
                return BadRequest($"Temporada con id '{id}' no encontrada.");
            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
