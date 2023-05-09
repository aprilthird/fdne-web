using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Models;
using FDNE.PE.WEB.ADMIN.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/clubes")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class ClubController : BaseApiController
    {
        public ClubController() : base()
        {
        }
        [AllowAnonymous]
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _context.Clubs
                .Select(x => new ClubDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Acronym = x.Acronym,
                    Address = x.Address,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber
                }).AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.Clubs.FindAsync(id);
            var result = new ClubDto()
            {
                Id = data.Id,
                Name = data.Name,
                Acronym = data.Acronym,
                Address = data.Address,
                Latitude = data.Latitude == 0 ? -12.0262674 : data.Latitude,
                Longitude = data.Longitude == 0 ? -77.1282085 : data.Longitude,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                UrlPicture = data.UrlPicture
            };
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(ClubDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var club = new Club()
            {
                Name = model.Name,
                Acronym = model.Acronym,
                Address = model.Address,
                Longitude = model.Longitude,
                Latitude = model.Latitude,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.CLUBS);
                club.UrlPicture = pathImage;
            }
        
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(Guid id, ClubDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var club = await _context.Clubs.FindAsync(id);
            club.Name = model.Name;
            club.Acronym = model.Acronym;
            club.Address = model.Address;
            club.Longitude = model.Longitude;
            club.Latitude = model.Latitude;
            club.Email = model.Email;
            club.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if (!string.IsNullOrEmpty(club.UrlPicture))
                {
                    DeleteImage(club.UrlPicture);
                }
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.CLUBS);
                club.UrlPicture = pathImage;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var club = await _context.Clubs.FirstOrDefaultAsync(x => x.Id == id);
            if (club == null)
                return BadRequest($"Club con Id '{id}' no encontrada.");
            if (!string.IsNullOrEmpty(club.UrlPicture))
                DeleteImage(club.UrlPicture);
            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
