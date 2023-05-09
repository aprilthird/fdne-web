using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDNE.PE.CORE.Helpers;
using FNDE.PE.WEB.PORTAL.Controllers;
using FNDE.PE.WEB.PORTAL.ViewModels.NewsViewModels;

namespace FDNE.PE.WEB.PORTAL.Controllers
{
    [RoutePrefix("noticias")]
    public class NoticiasController : BaseController
    {
        
        public NoticiasController() : base()
        {
        }
        [Route("")]
        // GET: Noticias
        public async Task<ActionResult> Listado()
        {
            var data = await _context.News.ToListAsync();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            var model = data.Select(x => new NewsViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Day = x.Date.ToString("dd"),
                Month = x.Date.ToString("MMMM"),
                Year = x.Date.ToString("yyyy"),
            }).ToList();
            return View(model);
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult> Detalle(Guid id)
        {
            var data = await _context.News.FindAsync(id);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            var model = new NewsDetailViewModel
            {
                Id = data.Id,
                Title = data.Title,
                Body = data.Body,
                Day = data.Date.ToString("dd"),
                Month = data.Date.ToString("MMMM"),
                Year = data.Date.ToString("yyyy"),
                ImageUrl = data.ImageUrl
            };
            model.ImageUrl = string.IsNullOrEmpty(model.ImageUrl)
                ? Url.Content("~/Content/src/img/slide1-1024.png") : ToImageStorageUrl(model.ImageUrl);
            return View(model);
        }

        public ActionResult JinetesMundo()
        {
            return View();
        }

        public ActionResult JineteDetalle()
        {
            return View();
        }
    }
}