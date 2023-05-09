using FDNE.PE.CORE.Helpers;
using FDNE.PE.WEB.ADMIN.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FDNE.PE.WEB.ADMIN.Controllers
{
    [Authorize]
    [RoutePrefix("perfil")]
    public class ProfileController : BaseController
    {
        public ProfileController() : base()
        {
        }

        [Route]
        public async Task<ActionResult> Index()
        {
            var user = await UserAsync;
            var detailedUser = await _context.Users
                .Include(x => x.DocumentType)
                .Where(x => x.Id == user.Id)
                .FirstOrDefaultAsync();
            var model = new ProfileDto
            {
                Name = detailedUser.Name,
                MaternalSurname = detailedUser.MaternalSurname,
                PaternalSurname = detailedUser.PaternalSurname,
                Document = detailedUser.Document,
                Email = detailedUser.Email,
                ImageUrl = detailedUser.UrlPicture,
                DocumentType = new DocumentTypeDto
                {
                    Name = detailedUser.DocumentType.Name
                }
            };
            return View(model);
        }

        [Route("editar")]
        public ActionResult Edit() => View(new ProfileDto());
    }
}
