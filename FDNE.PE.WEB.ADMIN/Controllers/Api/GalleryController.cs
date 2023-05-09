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
    [RoutePrefix("api/contenido/galeria")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class GalleryController : BaseApiController
    {
        public GalleryController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var data = await _context.Galleries.ToListAsync();
            var result = data.Select(x => new GalleryDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Date = x.Date.ToLocalDateTimeFormat()
            });
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Galleries.FindAsync(id);
            var result = new GalleryDto
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                ImageUrl = data.ImageUrl
            };
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(GalleryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var gallery = new Gallery()
            {
                Title = model.Title,
                Description = model.Description,
                Date = DateTime.UtcNow,
            };

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.GALLERY);
                gallery.ImageUrl = pathImage;
            }

            _context.Galleries.Add(gallery);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, GalleryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var gallery = await _context.Galleries.FindAsync(id);

            gallery.Title = model.Title;
            gallery.Description = model.Description;

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if(!string.IsNullOrEmpty(gallery.ImageUrl))
                {
                    DeleteImage(gallery.ImageUrl);
                }
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.GALLERY);
                gallery.ImageUrl = pathImage;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var gallery = await _context.Galleries.FirstOrDefaultAsync(x => x.Id == id);
            if (gallery == null)
                return BadRequest($"Galeria con Id '{id}' no encontrada.");
            if (!string.IsNullOrEmpty(gallery.ImageUrl))
            {
                DeleteImage(gallery.ImageUrl);
            }
            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
