using FDNE.PE.DATA.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FDNE.PE.WEB.ADMIN.Dtos;
using System.Data.Entity;
using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/jinetes")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN + "," + ConstantHelpers.ROLE.CLUB_ADMIN)]
    public class RiderController : BaseApiController
    {

        public RiderController() : base()
        {

        }
        
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(Guid? clubId = null)
        {
            var query = _context.Riders.Include(x => x.User)
                .Include(x => x.User.DocumentType)
                .Include(x => x.RiderClubs.Select(r => r.Club))
                .AsNoTracking().AsQueryable();
            if (clubId.HasValue)
                query = query.Where(x => x.RiderClubs.Any(r => r.ClubId == clubId.Value));
            if (User.IsInRole(ConstantHelpers.ROLE.CLUB_ADMIN))
            {
                var adminClub = await _context.ClubAdministrators.FirstOrDefaultAsync(x => x.UserId == UserId);
                query = query.Where(x => x.RiderClubs.Any(r => r.ClubId == adminClub.ClubId));
            }
            var data = await query.ToListAsync();
            var result = data.Select(x => new RiderDto
            {
                Id = x.UserId,
                User = new ApplicationUserDto
                {
                    DocumentTypeId = x.User.DocumentTypeId,
                    Document = x.User.Document,
                    DocumentType = new DocumentTypeDto
                    {
                        Name = x.User.DocumentType.Name
                    },
                    Name = x.User.Name,
                    MaternalSurname = x.User.MaternalSurname,
                    PaternalSurname = x.User.PaternalSurname,
                    Sex = x.User.Sex,
                    PhoneNumber = x.User.PhoneNumber,
                    BirthDate = x.User.BirthDate?.ToLocalDateFormat()
                },
                ClubIds = x.RiderClubs.Select(r => r.ClubId),
                Clubs = x.RiderClubs.Select(r => new ClubDto
                {
                    Name = r.Club.Name
                }).ToList(),
                IsActive = x.IsActive
            }); 
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var data = await _context.Riders.Include(x => x.User).Include(x => x.RiderClubs)
                .FirstOrDefaultAsync(x => x.UserId == id);

            var result = new RiderDto
            {
                Id = data.UserId,
                User = new ApplicationUserDto
                {
                    UserName = data.User.UserName,
                    Email = data.User.Email,
                    Document = data.User.Document,
                    DocumentTypeId = data.User.DocumentTypeId,
                    Name = data.User.Name,
                    PaternalSurname = data.User.PaternalSurname,
                    MaternalSurname = data.User.MaternalSurname,
                    PhoneNumber = data.User.PhoneNumber,
                    BirthDate = data.User.BirthDate?.ToLocalDateFormat(),
                    Sex = data.User.Sex
                },
                ClubIds = data.RiderClubs.Select(r => r.ClubId).ToList(),
                IsActive = data.IsActive
            };

            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(RiderDto model)
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

            if (User.IsInRole(ConstantHelpers.ROLE.ADMIN))
            {
                if (!model.ClubIds.Any())
                    return BadRequest($"El jinete debe pertenecer al menos a un club.");
            }

            var newUser = new ApplicationUser
            {
                UserName = model.User.UserName,
                Email = model.User.Email,                
                DocumentTypeId = model.User.DocumentTypeId,
                Document = model.User.Document,
                Name = model.User.Name,
                PaternalSurname = model.User.PaternalSurname,
                MaternalSurname = model.User.MaternalSurname,
                PhoneNumber = model.User.PhoneNumber,
                BirthDate = model.User.BirthDate?.ToUtcDateTime(),
                Sex = model.User.Sex,
                IsActive = model.IsActive
            };
            
            await _userManager.CreateAsync(newUser, model.User.Password);
            await _userManager.AddToRoleAsync(newUser.Id, ConstantHelpers.ROLE.RIDER);

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                string filePath = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.USERS);
                newUser.UrlPicture = filePath;
                await _context.SaveChangesAsync();
            }

            var riderClubs = new List<RiderClub>();
            var rider = new Rider()
            {
                UserId = newUser.Id,  
                IsActive = model.IsActive
            };

            if (User.IsInRole(ConstantHelpers.ROLE.CLUB_ADMIN))
            {
                var adminClub = await _context.ClubAdministrators.FirstOrDefaultAsync(x => x.UserId == UserId);
                riderClubs.Add(new RiderClub
                {
                    ClubId = adminClub.ClubId,
                    Rider = rider
                });
            }
            else
            {
                riderClubs.AddRange(model.ClubIds.Select(x => new RiderClub
                {
                    ClubId = x,
                    Rider = rider
                }));
            }

            _context.Riders.Add(rider);
            _context.RiderClubs.AddRange(riderClubs);
            await _context.SaveChangesAsync();
            return Ok();            
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(string id, RiderDto model)
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

            if (User.IsInRole(ConstantHelpers.ROLE.ADMIN))
            {
                if (!model.ClubIds.Any())
                    return BadRequest($"El jinete debe pertenecer al menos a un club.");
            }
            var rider = await _context.Riders.Include(x => x.User).Include(x => x.RiderClubs)
                .FirstOrDefaultAsync(x => x.UserId == id);

            rider.User.UserName = model.User.UserName;
            rider.User.Email = model.User.Email;
            rider.User.Name = model.User.Name;
            rider.User.PaternalSurname = model.User.PaternalSurname;
            rider.User.MaternalSurname = model.User.MaternalSurname;
            rider.User.DocumentTypeId = model.User.DocumentTypeId;
            rider.User.Document = model.User.Document;
            rider.User.PhoneNumber = model.User.PhoneNumber;
            rider.User.BirthDate = model.User.BirthDate?.ToUtcDateTime();
            rider.User.Sex = model.User.Sex;
            rider.User.IsActive = model.IsActive;
            
            if (User.IsInRole(ConstantHelpers.ROLE.ADMIN))
            {
                if (rider.RiderClubs.Any())
                    _context.RiderClubs.RemoveRange(rider.RiderClubs);
                _context.RiderClubs.AddRange(model.ClubIds.Select(x => new RiderClub
                {
                    ClubId = x,
                    Rider = rider
                }));
            }

            rider.IsActive = model.IsActive;

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if(!string.IsNullOrEmpty(rider.User.UrlPicture))
                    DeleteImage(rider.User.UrlPicture);
                string filePath = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.USERS);
                rider.User.UrlPicture = filePath;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var rider = await _context.Riders.Include(x => x.RiderClubs)
                .FirstOrDefaultAsync(x => x.UserId == id);
            if (rider == null)
                return BadRequest($"Jinete con Id '{id}' no encontrada.");
            if (rider.RiderClubs.Any())
                _context.RiderClubs.RemoveRange(rider.RiderClubs);
            _context.Riders.Remove(rider);
            var user = await _userManager.FindByIdAsync(id);
            if(!string.IsNullOrEmpty(rider.User.UrlPicture))
                DeleteImage(rider.User.UrlPicture);
            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
