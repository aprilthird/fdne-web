    using FNDE.PE.WEB.PORTAL.Controllers;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
using FNDE.PE.WEB.PORTAL.ViewModels.AboutUsViewModels;
using System.Configuration;
using FDNE.PE.CORE.Helpers;

namespace FDNE.PE.WEB.PORTAL.Controllers
{
    public class NosotrosController : BaseController
    {
        public NosotrosController() : base()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> MisionVision()
        {
            var model = await _context.FederationInformation
                .Select(x => new MisionVisionViewModel
                {
                    AboutUs = x.AboutUs,
                    Mision = x.Mision,
                    Vision = x.Vision
                }).FirstOrDefaultAsync();
            return View(model);
        }

        public async Task<ActionResult> Organizacion()
        {
            var model = await _context.FederationOrganization
            .Select(x => new OrganizationViewModel
            {
                Title = x.Title,
                Name = x.Name
            }).AsNoTracking().ToListAsync();
            return View(model);
        }

        public async Task<ActionResult> PautasTemporada()
        {

            var model = await _context.FederationGuidelines
               .Select(x => new GuideLinesViewModel
               {
                   Body = x.Body,
                   ImageUrl = x.ImageUrl
               }).FirstOrDefaultAsync();
            model.ImageUrl = string.IsNullOrEmpty(model.ImageUrl) 
                ? Url.Content("~/Content/src/img/slide1-1024.png") : ToImageStorageUrl(model.ImageUrl);
            return View(model);
        }

        public async Task<ActionResult> Historia()
        {
            var model = await _context.FederationHistories
                .Select(x => new HistoryViewModel
                {
                    ImageUrl = x.UrlImage,
                    InformationPerYear = _context.HistoryPerYears.Select(y => new HistoryPerYear
                    {
                        Year = y.Year,
                        Body = y.Body
                    }).ToList()
                }).AsNoTracking().FirstOrDefaultAsync();
            model.ImageUrl = string.IsNullOrEmpty(model.ImageUrl)
            ? Url.Content("~/Content/src/img/slide4-1024.png") : ToImageStorageUrl(model.ImageUrl);
            return View(model);
        }

        public async Task<ActionResult> Oficiales()
        {
            var model = await _context.FederationOfficers
                .Select(x => new OfficersViewModel
                {
                    ImageUrl = x.UrlImage,
                    LstOfficers = _context.Officers.Select(y => new ChildOfficersViewModel
                    {
                        Name = y.Name,
                        CategoryName = y.Category,
                        DisciplineName = y.Discipline? "Salto":"Adiestramiento"
                    }).ToList()
                }).AsNoTracking().FirstOrDefaultAsync();
            model.ImageUrl = string.IsNullOrEmpty(model.ImageUrl)
                ? Url.Content("~/Content/src/img/slide1-1024.png") : ToImageStorageUrl(model.ImageUrl);
            return View(model);
        }
    }
}