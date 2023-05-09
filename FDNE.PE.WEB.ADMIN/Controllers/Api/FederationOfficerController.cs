using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Models;
using FDNE.PE.WEB.ADMIN.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/federation-officers")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class FederationOfficerController : BaseApiController
    {
        public FederationOfficerController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var federationOfficer = await _context.FederationOfficers
                .AsNoTracking().FirstOrDefaultAsync();

            if (federationOfficer == null)
            {
                federationOfficer = new FederationOfficers()
                {
                    UrlImage = ""
                };
                _context.FederationOfficers.Add(federationOfficer);
                await _context.SaveChangesAsync();
            }

            var result = new FederationOfficersDto
            {
                Id = federationOfficer.Id,
                ImageUrl = federationOfficer.UrlImage
            };
            return Ok(result);

        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(FederationOfficersDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var federationOfficer = await _context.FederationOfficers.FindAsync(model.Id);

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if (!string.IsNullOrEmpty(federationOfficer.UrlImage))
                {
                    DeleteImage(federationOfficer.UrlImage);
                }
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.GENERAL);               
                federationOfficer.UrlImage = pathImage;
            }

            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpGet, Route("oficial")]
        public async Task<IHttpActionResult> GetOfficer()
        {
            var data = await _context.Officers.AsNoTracking().ToListAsync();

            var result = data.Select(x => new OfficersDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryName = x.Category,
                DisciplinaName = Convert.ToByte(x.Discipline)
            });
            
            return Ok(result);
        }

        [HttpGet, Route("oficial/{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Officers.FindAsync(id);
            var result = new OfficersDto()
            {
                Id = data.Id,
                Name = data.Name,
                CategoryName = data.Category,
                DisciplinaName = Convert.ToByte(data.Discipline)  
            };
            return Ok(result);
        }

        [HttpPost, Route("oficial")]
        public async Task<IHttpActionResult> Create(OfficersDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var federationOfficer = await _context.FederationOfficers
                .AsNoTracking().FirstOrDefaultAsync();

            var officer = new Officers()
            {
                Name = model.Name,
                Category = model.CategoryName,
                Discipline = model.DisciplinaName == 1 ? true : false,
                FederationOfficersId = federationOfficer.Id
            };
            _context.Officers.Add(officer);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut, Route("oficial/{id}")]
        public async Task<IHttpActionResult> OffircerUpdate(Guid id, OfficersDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var officer = await _context.Officers.FindAsync(id);
            officer.Name = model.Name;
            officer.Category = model.CategoryName;
            officer.Discipline = model.DisciplinaName == 1 ? true : false;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("oficial/{id}")]
        public async Task<IHttpActionResult> OfficerDelete(Guid id)
        {
            var officer = await _context.Officers.FirstOrDefaultAsync(x => x.Id == id);
            if (officer == null)
                return BadRequest($"Oficial con Id '{id}' no encontrada.");
            _context.Officers.Remove(officer);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
