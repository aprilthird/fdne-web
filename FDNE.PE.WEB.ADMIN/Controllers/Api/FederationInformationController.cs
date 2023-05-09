using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FDNE.PE.WEB.ADMIN.Dtos;
using FDNE.PE.DATA.Models;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/contenido/informacion-general")]
    [System.Web.Http.Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class FederationInformationController : BaseApiController
    {
        public FederationInformationController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var federationInformation = await _context.FederationInformation
                .AsNoTracking().FirstOrDefaultAsync();
            if (federationInformation == null)
            {
                federationInformation = new FederationInformation()
                {
                    Mision = "Texto por Defecto",
                    Vision = "Texto por Defecto",
                    AboutUs = "Texto por Defecto"
                };
                _context.FederationInformation.Add(federationInformation);
                await _context.SaveChangesAsync();
            }
            var result = new FederationInformationDto
            {
                Mision = federationInformation.Mision,
                Vision = federationInformation.Vision,
                AboutUs = federationInformation.AboutUs
            };
            return Ok(result);
        }

        [HttpPut, Route("")]
        public async Task<IHttpActionResult> Update(FederationInformationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var federationInformation = await _context.FederationInformation.FirstOrDefaultAsync();
            federationInformation.Mision = model.Mision;
            federationInformation.Vision = model.Vision;
            federationInformation.AboutUs = model.AboutUs;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
