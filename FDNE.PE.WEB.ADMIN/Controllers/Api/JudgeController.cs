using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Models;
using FDNE.PE.WEB.ADMIN.Dtos;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/jueces")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class JudgeController : BaseApiController
    {
        public JudgeController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var rol = await roleManager.FindByNameAsync("Jurado");
            
            var data = await _context.Users.Include(x => x.Roles)
                .Include(x => x.DocumentType)
                .Where(x => x.Roles.Any(y => y.RoleId == rol.Id))
                .AsNoTracking().ToListAsync();

            var result = data.Select(x => new ApplicationUserDto
            {
                Id = new Guid(x.Id),
                Document = x.Document,
                DocumentTypeId = x.DocumentTypeId,
                DocumentType = new DocumentTypeDto
                {
                    Name = x.DocumentType.Name
                },
                Name = x.Name,
                MaternalSurname = x.MaternalSurname,
                PaternalSurname = x.PaternalSurname,
                Sex = x.Sex,
                PhoneNumber = x.PhoneNumber,
                BirthDate = x.BirthDate?.ToLocalDateFormat(),
                IsActive = x.IsActive
            });

            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userDto = new ApplicationUserDto
            {
                Id = new Guid(user.Id),
                UserName = user.UserName,
                Email = user.Email,
                DocumentTypeId = user.DocumentTypeId,
                Document = user.Document,
                Name = user.Name,
                PaternalSurname = user.PaternalSurname,
                MaternalSurname = user.MaternalSurname,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate?.ToLocalDateFormat(),
                Sex = user.Sex,
                IsActive = user.IsActive
            };

            return Ok(userDto);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(ApplicationUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                DocumentTypeId = model.DocumentTypeId,
                Document = model.Document,
                Name = model.Name,
                PaternalSurname = model.PaternalSurname,
                MaternalSurname = model.MaternalSurname,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate?.ToUtcDateTime(),
                Sex = model.Sex,
                IsActive = model.IsActive
            };
           
            await _userManager.CreateAsync(newUser, model.Password);
            await _userManager.AddToRoleAsync(newUser.Id, ConstantHelpers.ROLE.JUDGE);

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                string filePath = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.USERS);
                newUser.UrlPicture = filePath;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(string id, ApplicationUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id);

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Name = model.Name;
            user.PaternalSurname = model.PaternalSurname;
            user.MaternalSurname = model.MaternalSurname;
            user.DocumentTypeId = model.DocumentTypeId;
            user.Document = model.Document;
            user.PhoneNumber = model.PhoneNumber;
            user.BirthDate = model.BirthDate?.ToUtcDateTime();
            user.Sex = model.Sex;
            user.IsActive = model.IsActive;

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                DeleteFile(user.UrlPicture);
                string filePath = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.USERS);
                user.UrlPicture = filePath;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest($"Usuario con Id '{id}' no encontrada.");
            DeleteFile(user.UrlPicture);
            await _userManager.DeleteAsync(user);
            return Ok();
        }
    }
}
