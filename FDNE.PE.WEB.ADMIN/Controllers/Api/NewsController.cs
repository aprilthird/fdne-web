using FDNE.PE.CORE.Helpers;
using FDNE.PE.WEB.ADMIN.Dtos;
using FDNE.PE.DATA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{

    [RoutePrefix("api/contenido/noticias")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class NewsController : BaseApiController
    {
        public NewsController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var data = await _context.News.ToListAsync();
            var result = data.Select(x => new NewsDto
            {
                Id = x.Id,
                Title = x.Title,
                Body = x.Body,
                ImageUrl = x.ImageUrl,
                Date = x.Date.ToLocalDateTimeFormat()
            });
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.News.FindAsync(id);
            var result = new NewsDto()
            {
                Id = data.Id,
                Title = data.Title,
                Body = data.Body,
                ImageUrl = data.ImageUrl
            };
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(NewsDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var news = new News()
            {
                Title = model.Title,
                Body = model.Body,
                Date = DateTime.UtcNow
            };

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.NEWS);
                news.ImageUrl= pathImage;
            }

            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, NewsDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var news = await _context.News.FindAsync(id);
            news.Title = model.Title;
            news.Body = model.Body;
            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if (!string.IsNullOrEmpty(news.ImageUrl))
                {
                    DeleteImage(news.ImageUrl);
                }
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.NEWS);
                news.ImageUrl = pathImage;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var news = await _context.News.FirstOrDefaultAsync(x => x.Id == id);
            if (news == null)
                return BadRequest($"Noticia con Id '{id}' no encontrada.");
            if (!string.IsNullOrEmpty(news.ImageUrl))
            {
                DeleteImage(news.ImageUrl);
            }
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
