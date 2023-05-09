using FDNE.PE.CORE.Helpers;
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
    [AllowAnonymous]
    [RoutePrefix("api/tipos-de-documento")]
    public class DocumentTypeController : BaseApiController
    {
        public DocumentTypeController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _context.DocumentTypes
                .Select(x => new DocumentTypeDto
                {
                    Id = x.Id,
                    Name = x.Acronym
                }).AsNoTracking().ToListAsync();
            return Ok(result);
        }
    }
}
