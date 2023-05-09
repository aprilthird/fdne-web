using FNDE.PE.WEB.PORTAL.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FNDE.PE.WEB.PORTAL.ViewModels.GalleryViewModels;

namespace FDNE.PE.WEB.PORTAL.Controllers
{
    public class GaleriaController : BaseController
    {
        public GaleriaController() : base()
        {

        } 


        // GET: Galeria
        public async Task<ActionResult> Listado()
        {
            var model = await _context.Galleries
                .Select(x => new PictureViewModel
                {
                    Title = x.Title,
                    ImageUrl = x.ImageUrl
                }).ToListAsync();
            foreach(var g in model)
                g.ImageUrl = ToImageStorageUrl(g.ImageUrl);
            return View(model);
        }

        public ActionResult Detalle()
        {
            return View();
        }
    }
}