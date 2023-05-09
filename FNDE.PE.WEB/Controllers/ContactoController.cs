using FNDE.PE.WEB.PORTAL.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FNDE.PE.WEB.PORTAL.ViewModels.ContactViewModels;


namespace FDNE.PE.WEB.PORTAL.Controllers
{
    public class ContactoController : BaseController
    {
        public ContactoController() : base()
        {

        }

         public ActionResult Index()
        {
            return View();
        }
        // GET: Contacto


        public ActionResult Agradecimiento()
        {
            return View();
        }

        public async Task<ActionResult> Contacto()
        {
            var model = await _context.FederationContact
                .Select(x => new ContactoViewModel
                {
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                }).FirstOrDefaultAsync();
                return View(model);
        }
    }
}