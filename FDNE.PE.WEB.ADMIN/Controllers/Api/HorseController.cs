using FDNE.PE.CORE.Helpers;
using FDNE.PE.WEB.ADMIN.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using FDNE.PE.DATA.Models;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/caballos")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN + "," + ConstantHelpers.ROLE.CLUB_ADMIN)]
    public class HorseController : BaseApiController
    {
        public HorseController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll(Guid? clubId = null)
        {
            var query = _context.Horses.Include(x => x.HorseClubs.Select(h => h.Club))
                .AsNoTracking().AsQueryable();
            if (clubId.HasValue)
                query = query.Where(x => x.HorseClubs.Any(h => h.ClubId == clubId.Value));
            if (User.IsInRole(ConstantHelpers.ROLE.CLUB_ADMIN))
            {
                var adminClub = await _context.ClubAdministrators.FirstOrDefaultAsync(x => x.UserId == UserId);
                query = query.Where(x => x.HorseClubs.Any(h => h.ClubId == adminClub.ClubId));
            }
            var data = await query.ToListAsync();
            var result = data.Select(x => new HorseDto
            {
                Id = x.Id,
                ClubIds = x.HorseClubs.Select(h => h.ClubId).ToList(),
                Clubs = x.HorseClubs.Select(h => new ClubDto
                {
                    Name = h.Club.Name
                }).ToList(),
                Name = x.Name,
                Sex = x.Sex,
                BelongsToUs = x.BelongsToUs               
            });
            return Ok(result);
        }
        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Horses.Include(h => h.HorseClubs)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (data == null)
                return BadRequest($"Caballo con Id '{id}' no encontrada.");
            var result = new HorseDto
            {
                Id = data.Id,
                ClubIds = data.HorseClubs.Select(h => h.ClubId).ToList(),
                Name = data.Name,
                Sex = data.Sex,
                BelongsToUs = data.BelongsToUs,
                IsActive = data.IsActive,
                UrlToImage = data.UrlToImage
            };

            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(HorseDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (User.IsInRole(ConstantHelpers.ROLE.ADMIN))
            {
                if (!model.ClubIds.Any())
                    return BadRequest($"El caballo debe pertenecer al menos a un club.");
            }
            
            var horseClubs = new List<HorseClub>();
            var horse = new Horse()
            {
                Name = model.Name,
                Sex = model.Sex,
                IsActive = model.IsActive,
                BelongsToUs = model.BelongsToUs,
            };

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.HORSE);
                horse.UrlToImage = pathImage;
            }

            if (User.IsInRole(ConstantHelpers.ROLE.CLUB_ADMIN))
            {
                var adminClub = await _context.ClubAdministrators.FirstOrDefaultAsync(x => x.UserId == UserId);
                horseClubs.Add(new HorseClub
                {
                    ClubId = adminClub.ClubId,
                    Horse = horse
                });
            }
            else
            {
                horseClubs.AddRange(model.ClubIds.Select(x => new HorseClub
                {
                    ClubId = x,
                    Horse = horse
                }));
            }

            _context.Horses.Add(horse);
            _context.HorseClubs.AddRange(horseClubs);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, HorseDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (User.IsInRole(ConstantHelpers.ROLE.ADMIN))
            {
                if (!model.ClubIds.Any())
                    return BadRequest($"El caballo debe pertenecer al menos a un club.");
            }
            var horse = await _context.Horses.Include(x => x.HorseClubs)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            horse.Name = model.Name;

            if (User.IsInRole(ConstantHelpers.ROLE.ADMIN))
            {
                if (horse.HorseClubs.Any())
                    _context.HorseClubs.RemoveRange(horse.HorseClubs);
                _context.HorseClubs.AddRange(model.ClubIds.Select(x => new HorseClub
                {
                    ClubId = x,
                    Horse = horse
                }));
            }

            horse.Sex = model.Sex;
            horse.IsActive = model.IsActive;
            horse.BelongsToUs = model.BelongsToUs;
            
            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if(!string.IsNullOrEmpty(horse.UrlToImage))
                {
                    DeleteImage(horse.UrlToImage);
                }
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.HORSE);
                horse.UrlToImage = pathImage;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var horse = await _context.Horses.Include(x => x.HorseClubs)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (horse == null)
                return BadRequest($"Caballo con Id '{id}' no encontrada.");
            if (horse.HorseClubs.Any())
                _context.HorseClubs.RemoveRange(horse.HorseClubs);
            if(!string.IsNullOrEmpty(horse.UrlToImage))
                DeleteImage(horse.UrlToImage);
            _context.Horses.Remove(horse);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
