using FDNE.PE.DATA.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FDNE.PE.CORE.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;
using FDNE.PE.DATA.Models;
using FDNE.PE.WEB.ADMIN.Dtos;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/categorias")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class CategoryController : BaseApiController
    {
        public CategoryController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(Guid? disciplineId = null)
        {
            var query = _context.Categories
                .Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Acronym = x.Acronym,
                    DisciplineId = x.DisciplineId,
                    Discipline = new DisciplineDto
                    {
                        Name = x.Discipline.Name
                    }
                }).AsQueryable();
            if(disciplineId.HasValue)
                query = query.Where(x => x.DisciplineId == disciplineId.Value);
            var result = await query.AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Categories.FindAsync(id);
            var result = new CategoryDto()
            {
                Id = data.Id,
                Name = data.Name,
                Acronym = data.Acronym,
                DisciplineId = data.DisciplineId
            };
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(CategoryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = new Category()
            {
                Name = model.Name,
                Acronym = model.Acronym,
                DisciplineId = model.DisciplineId
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, CategoryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = await _context.Categories.FindAsync(id);
            category.Name = model.Name;
            category.Acronym = model.Acronym;
            category.DisciplineId = model.DisciplineId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return BadRequest($"Categorìa con Id '{id}' no encontrada.");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
