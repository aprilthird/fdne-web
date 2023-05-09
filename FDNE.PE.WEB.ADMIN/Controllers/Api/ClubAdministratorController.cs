using FDNE.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using FDNE.PE.WEB.ADMIN.Dtos;
using FDNE.PE.DATA.Models;
using Microsoft.AspNet.Identity;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/administradores-de-club")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class ClubAdministratorController : BaseApiController
    {
        public ClubAdministratorController() : base()
        {
        }

        [HttpGet,Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var data = await _context.ClubAdministrators.Include(x => x.User)
                .Include(x => x.Club).Include(x => x.User.DocumentType)
                .AsNoTracking().ToListAsync();
            var result = data.Select(x => new ClubAdministratorDto
            {
                Id = x.UserId,
                User = new ApplicationUserDto
                {
                    Document = x.User.Document,
                    DocumentTypeId = x.User.DocumentTypeId,
                    DocumentType = new DocumentTypeDto
                    {
                        Name = x.User.DocumentType.Name
                    },
                    Name = x.User.Name,
                    MaternalSurname = x.User.MaternalSurname,
                    PaternalSurname = x.User.PaternalSurname,
                    Sex = x.User.Sex,
                    PhoneNumber = x.User.PhoneNumber,
                    BirthDate = x.User.BirthDate?.ToLocalDateFormat(),
                    IsActive = x.User.IsActive
                },
                ClubId = x.ClubId,
                Club = new ClubDto
                {
                    Name = x.Club.Name,
                }
            });
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var data = await _context.ClubAdministrators.Include(x => x.User)
                .Include(x => x.Club).Include(x => x.User.DocumentType)
                .FirstOrDefaultAsync(x => x.UserId == id);

            var result = new ClubAdministratorDto
            {
                Id = data.UserId,
                User = new ApplicationUserDto
                {
                    UserName = data.User.UserName,
                    Email = data.User.Email,
                    Document = data.User.Document,
                    DocumentTypeId = data.User.DocumentTypeId,
                    DocumentType = new DocumentTypeDto
                    {
                        Name = data.User.DocumentType.Name
                    },
                    Name = data.User.Name,
                    PaternalSurname = data.User.PaternalSurname,
                    MaternalSurname = data.User.MaternalSurname,
                    PhoneNumber = data.User.PhoneNumber,
                    BirthDate = data.User.BirthDate?.ToLocalDateFormat(),
                    Sex = data.User.Sex,
                    IsActive = data.User.IsActive
                },
                ClubId = data.ClubId,
                Club = new ClubDto
                {
                    Name = data.Club.Name
                }
            };

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost, Route]
        public async Task<IHttpActionResult> Create(ClubAdministratorDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var documentType = await _context.DocumentTypes.FindAsync(model.User.DocumentTypeId);
            var documentTypeErros = ValidateDocument(documentType, model.User.Document);
            if (documentTypeErros.Any())
                return BadRequest($"Su documento no cumple con las especificaciones de un {documentType.Name}: <br/></ul><li>" + string.Join("</li><li>", documentTypeErros) + "</li></ul>");
            if (await _context.Users.AnyAsync(x => x.UserName == model.User.UserName))
                return BadRequest("Ya existe otro usuario con el mismo usuario.");
            if (await _context.Users.AnyAsync(x => x.Document == model.User.Document))
                return BadRequest("Ya existe otro usuario con el mismo documento registrado.");

            var newUser = new ApplicationUser
            {
                UserName = model.User.UserName,
                Email = model.User.Email,
                Document = model.User.Document,
                DocumentTypeId = model.User.DocumentTypeId,
                Name = model.User.Name,
                PaternalSurname = model.User.PaternalSurname,
                MaternalSurname = model.User.MaternalSurname,
                PhoneNumber = model.User.PhoneNumber,
                BirthDate = model.User.BirthDate?.ToUtcDateTime(),
                Sex = model.User.Sex,
                IsActive = model.User.IsActive
            };

            if(!User.Identity.IsAuthenticated)
            {
                newUser.IsActive = false;
            }

            await _userManager.CreateAsync(newUser, model.User.Password);
            await _userManager.AddToRoleAsync(newUser.Id, ConstantHelpers.ROLE.CLUB_ADMIN);

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                string filePath = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.USERS);
                newUser.UrlPicture = filePath;
                await _context.SaveChangesAsync();
            }

            var clubAdministrator = new ClubAdministrator()
            {
                UserId = newUser.Id,
                ClubId = model.ClubId
            };
            _context.ClubAdministrators.Add(clubAdministrator);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(string id, ClubAdministratorDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var documentType = await _context.DocumentTypes.FindAsync(model.User.DocumentTypeId);
            var documentTypeErros = ValidateDocument(documentType, model.User.Document);
            if (documentTypeErros.Any())
                return BadRequest($"Su documento no cumple con las especificaciones de un {documentType.Name}: <br/></ul><li>" + string.Join("</li><li>", documentTypeErros) + "</li></ul>");
            if (await _context.Users.AnyAsync(x => id != x.Id && x.UserName == model.User.UserName))
                return BadRequest("Ya existe otro usuario con el mismo usuario.");
            if (await _context.Users.AnyAsync(x => id != x.Id && x.Document == model.User.Document))
                return BadRequest("Ya existe otro usuario con el mismo documento registrado.");

            var clubAdministrator = await _context.ClubAdministrators.Include(x => x.User)
                .Include(x => x.Club).FirstOrDefaultAsync(x => x.UserId == id);

            clubAdministrator.User.UserName = model.User.UserName;
            clubAdministrator.User.Email = model.User.Email;
            clubAdministrator.User.Name = model.User.Name;
            clubAdministrator.User.PaternalSurname = model.User.PaternalSurname;
            clubAdministrator.User.MaternalSurname = model.User.MaternalSurname;
            clubAdministrator.User.DocumentTypeId = model.User.DocumentTypeId;
            clubAdministrator.User.Document = model.User.Document;
            clubAdministrator.User.PhoneNumber = model.User.PhoneNumber;
            clubAdministrator.User.BirthDate = model.User.BirthDate?.ToUtcDateTime();
            clubAdministrator.User.Sex = model.User.Sex;
            clubAdministrator.User.IsActive = model.User.IsActive;

            clubAdministrator.ClubId = model.ClubId;

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if(!string.IsNullOrEmpty(clubAdministrator.User.UrlPicture))
                {
                    DeleteImage(clubAdministrator.User.UrlPicture);
                }
                string filePath = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.USERS);
                clubAdministrator.User.UrlPicture = filePath;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var clubAdministrador = await _context.ClubAdministrators.FirstOrDefaultAsync(x => x.UserId == id);
            if (clubAdministrador == null)
                return BadRequest($"Usuario con Id '{id}' no encontrada.");
            _context.ClubAdministrators.Remove(clubAdministrador);
            var user = await _userManager.FindByIdAsync(id);
            if(!string.IsNullOrEmpty(user.UrlPicture))
                DeleteImage(user.UrlPicture);
            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
