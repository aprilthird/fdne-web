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
    [RoutePrefix("api/contenido/contacto")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class FederationContactController : BaseApiController
    {
        public FederationContactController() : base()
        {

        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var federationContact = await _context.FederationContact
                .AsNoTracking().FirstOrDefaultAsync();
            if (federationContact == null)
            {
                federationContact = new FederationContact()
                {
                    Address = "Direccion por defecto",
                    Latitude = -12.0262674,
                    Longitude = -77.1282085
                };
                _context.FederationContact.Add(federationContact);
                await _context.SaveChangesAsync();
            }
            var result = new FederationContactDto
            {
                Address = federationContact.Address,
                Latitude = federationContact.Latitude,
                Longitude = federationContact.Longitude
            };
            return Ok(result);
        }

        [HttpPut, Route("")]
        public async Task<IHttpActionResult> Update(FederationContactDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var federationContact = await _context.FederationContact.FirstOrDefaultAsync();
            federationContact.Address = model.Address;
            federationContact.Latitude = model.Latitude;
            federationContact.Longitude = model.Longitude;
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
