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
using Microsoft.Ajax.Utilities;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/niveles")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class LevelController : BaseApiController
    {
        public LevelController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(Guid? disciplineId = null)
        {
            var query = _context.Levels
                .Select(x => new LevelDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DisciplineId = x.DisciplineId,
                    Discipline = new DisciplineDto
                    {
                        Name = x.Discipline.Name
                    }
                }).AsQueryable();
            if (disciplineId.HasValue)
                query = query.Where(x => x.DisciplineId == disciplineId.Value);
            var result = await query.AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Levels.FindAsync(id);
            var result = new LevelDto()
            {
                Id = data.Id,
                Name = data.Name,
                DisciplineId = data.DisciplineId
            };
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(LevelDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var level = new Level()
            {
                Name = model.Name,
                DisciplineId = model.DisciplineId
            };
            _context.Levels.Add(level);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, LevelDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var level = await _context.Levels.FindAsync(id);
            level.Name = model.Name;
            level.DisciplineId = model.DisciplineId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var level = await _context.Levels.FirstOrDefaultAsync(x => x.Id == id);
            if (level == null)
                return BadRequest($"Nivel con Id '{id}' no encontrada.");
            _context.Levels.Remove(level);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
