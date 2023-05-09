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
    [RoutePrefix("api/perfil")]
    [Authorize]
    public class ProfileController : BaseApiController
    {
        public ProfileController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var profile = await UserAsync;
            var result = new ProfileDto
            {
                Name = profile.Name,
                PaternalSurname = profile.PaternalSurname,
                MaternalSurname = profile.MaternalSurname,
                Email = profile.Email,
                Document = profile.Document,
                DocumentTypeId = profile.DocumentTypeId
            };
            return Ok(result);
        }


        [HttpPut, Route("")]
        public async Task<IHttpActionResult> Update(ProfileDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var profile = await UserAsync;
            profile.Name = model.Name;
            profile.PaternalSurname = model.PaternalSurname;
            profile.MaternalSurname = model.MaternalSurname;
            profile.Email = model.Email;
            profile.Document = model.Document;
            profile.DocumentTypeId = model.DocumentTypeId;
            await _context.SaveChangesAsync();
            return Ok();
        }



    }
}
