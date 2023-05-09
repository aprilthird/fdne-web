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
    [RoutePrefix("api/contenido/pautas")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class FederationGuidelinesController : BaseApiController
    {
        public FederationGuidelinesController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var federationGuidelines = await _context.FederationGuidelines
                .AsNoTracking().FirstOrDefaultAsync();
            if (federationGuidelines == null)
            {
                federationGuidelines = new FederationGuidelines()
                {
                    Body = "Texto por Defecto",
                    ImageUrl = ""
                };
                _context.FederationGuidelines.Add(federationGuidelines);
                await _context.SaveChangesAsync();
            }
            var result = new FederationGuidelinesDto
            {
                Id = federationGuidelines.Id,
                Body = federationGuidelines.Body,
                ImageUrl = federationGuidelines.ImageUrl
            };
            return Ok(result);
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(FederationGuidelinesDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var federationGuidelines = await _context.FederationGuidelines.FirstOrDefaultAsync();

            federationGuidelines.Body = model.Body;

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if(!string.IsNullOrEmpty(federationGuidelines.ImageUrl))
                {
                    DeleteImage(federationGuidelines.ImageUrl);
                }
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.GENERAL);
                federationGuidelines.ImageUrl = pathImage;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
