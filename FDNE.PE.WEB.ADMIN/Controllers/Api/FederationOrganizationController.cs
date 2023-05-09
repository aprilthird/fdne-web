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
    [RoutePrefix("api/organizaciones")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class FederationOrganizationController : BaseApiController
    {
        public FederationOrganizationController() : base()
        {    
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var data = await _context.FederationOrganization.AsNoTracking().ToListAsync();
            var result = data.Select(x => new FederationOrganizationDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Name = x.Name
                });
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.FederationOrganization.FindAsync(id);
            var result = new FederationOrganizationDto
            {
                Id = data.Id,
                Title = data.Title,
                Name = data.Name
            };
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(FederationOrganizationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var organization = new FederationOrganization()
            {
                Title = model.Title,
                Name = model.Name
            };
            _context.FederationOrganization.Add(organization);
            await _context.SaveChangesAsync();
            return Ok(organization);
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, FederationOrganizationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var organization = await _context.FederationOrganization.FindAsync(id);
            organization.Title = model.Title;
            organization.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok(organization);
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var organization = await _context.FederationOrganization.FirstOrDefaultAsync(x => x.Id == id);
            if (organization == null)
                return BadRequest($"Organización con id '{id}' no encontrada.");
            _context.FederationOrganization.Remove(organization);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
