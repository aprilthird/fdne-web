using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Context;
using FDNE.PE.DATA.Models;
using FDNE.PE.WEB.ADMIN.Dtos;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/disciplinas")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class DisciplineController : BaseApiController
    {
        public DisciplineController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _context.Disciplines
                .Select(x => new DisciplineDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .AsNoTracking()
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Disciplines.FindAsync(id);
            var result = new DisciplineDto
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description
            };
            return Ok(result);
        }


        [HttpGet, Route("portal/{id}")]
        public async Task<IHttpActionResult> GetPortal(Guid id)
        {
            var model = await _context.DisciplinePortals.AsNoTracking().FirstOrDefaultAsync(x => x.DisciplineId == id);
            if (model == null)
            {
                model = new DisciplinePortal()
                {
                    DisciplineId = id,
                    FirstTab = "Primer tab",
                    FirstBody = "Primer Cuerpo",
                    SecondTab = "Segundo tab",
                    SecondBody = "Segundo Cuerpo",
                    ThirdTab = "Tercer tab",
                    ThirdBody = "Tercer Cuerpo",
                    QuarterTab = "Cuarto tab",
                    QuarterBody = "Cuarto Cuerpo",
                    FifthTab = "Quinto tab",
                    FifthBody = "Quinto Cuerpo",
                    SixthTab = "Sexto tab",
                    SixthBody = "Sexto Cuerpo",
                    UrlImage = ""
                };
                _context.DisciplinePortals.Add(model);
                await _context.SaveChangesAsync();
            }
            var result = new DisciplinePortalDto()
            {
                DisciplineId = id,
                FirstTab = model.FirstTab,
                FirstBody = model.FirstTab,
                SecondTab = model.SecondTab,
                SecondBody = model.SecondBody,
                ThirdTab = model.ThirdTab,
                ThirdBody = model.ThirdBody,
                QuarterTab = model.QuarterTab,
                QuarterBody = model.QuarterBody,
                FifthTab = model.FifthTab,
                FifthBody = model.FifthBody,
                SixthTab = model.SixthTab,
                SixthBody = model.SixthBody,
                ImageUrl = model.UrlImage
            };
            return Ok(result);
        }

        [HttpPut, Route("portal/{id}")]
        public async Task<IHttpActionResult> Update(Guid id, DisciplinePortalDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var disciplinePortal = await _context.DisciplinePortals.FirstOrDefaultAsync(x => x.DisciplineId == id);

            disciplinePortal.FirstTab = model.FirstTab;
            disciplinePortal.FirstBody = model.FirstTab;
            disciplinePortal.SecondTab = model.SecondTab;
            disciplinePortal.SecondBody = model.SecondBody;
            disciplinePortal.ThirdTab = model.ThirdTab;
            disciplinePortal.ThirdBody = model.ThirdBody;
            disciplinePortal.QuarterTab = model.QuarterTab;
            disciplinePortal.QuarterBody = model.QuarterBody;
            disciplinePortal.FifthTab = model.FifthTab;
            disciplinePortal.FifthBody = model.FifthBody;
            disciplinePortal.SixthTab = model.SixthTab;
            disciplinePortal.SixthBody = model.SixthBody;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(DisciplineDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var discipline = new Discipline
            {
                Name = model.Name,
                Description = model.Description
            };
            _context.Disciplines.Add(discipline);
            await _context.SaveChangesAsync();
            return Ok(discipline);
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, DisciplineDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var discipline = await _context.Disciplines.FindAsync(id);
            discipline.Name = model.Name;
            discipline.Description = model.Description;
            await _context.SaveChangesAsync();
            return Ok(discipline);
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var discipline = await _context.Disciplines.FirstOrDefaultAsync(x => x.Id == id);
            if (discipline == null)
                return BadRequest($"Disciplina con id '{id}' no encontrada.");
            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
