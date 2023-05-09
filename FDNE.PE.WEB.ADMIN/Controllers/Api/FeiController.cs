using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Models;
using FDNE.PE.WEB.ADMIN.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/federation-fei")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class FeiController : BaseApiController
    {
        public FeiController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var federationFEI = await _context.FederationFEIs
                .AsNoTracking().FirstOrDefaultAsync();

            if (federationFEI == null)
            {
                federationFEI = new FederationFEI()
                {
                    Title = "Texto de ejemplo",
                    SubTitle = "Texto de ejemplo",
                    Body = "Texto de ejemplo",
                    ImageUrl = "",                   
                    FileUrl = ""
                };
                _context.FederationFEIs.Add(federationFEI);
                await _context.SaveChangesAsync();
            }

            var result = new FederationFEIDto
            {
                Id = federationFEI.Id,
                Title = federationFEI.Title,
                SubTitle = federationFEI.SubTitle,
                Body = federationFEI.Body,
                ImageUrl = federationFEI.ImageUrl,
                FileUrl = federationFEI.FileUrl
            };
            return Ok(result);

        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(FederationFEIDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var federationFEI = await _context.FederationFEIs.FindAsync(model.Id);

            federationFEI.Title = model.Title;
            federationFEI.SubTitle = model.SubTitle;
            federationFEI.Body = model.Body;

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if (!string.IsNullOrEmpty(federationFEI.ImageUrl))
                {
                    DeleteImage(federationFEI.ImageUrl);
                }
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.GENERAL);

                federationFEI.ImageUrl = pathImage;
            }

            if (!string.IsNullOrEmpty(model.Base64File))
            {
                if (!string.IsNullOrEmpty(federationFEI.FileUrl))
                {
                    DeleteFile(federationFEI.FileUrl);
                }
                string pathFile = SaveFile(model.Base64File, model.Extension);

                federationFEI.FileUrl = pathFile;
            }

            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpGet, Route("archivos")]
        public async Task<IHttpActionResult> GetFileAll()
        {
            var data = await _context.FEIFiles.AsNoTracking().ToListAsync();

            var result = data.Select(x => new FeiFilesDto
            {
                Id = x.Id,
                Name = x.Nanme                
            });

            return Ok(result);
        }

        [HttpGet, Route("archivos/{id}")]
        public async Task<IHttpActionResult> GetFile(Guid id)
        {
            var data = await _context.FEIFiles.FindAsync(id);

            var result = new FeiFilesDto()
            {
                Id = data.Id,
                Name = data.Nanme
            };
            return Ok(result);
        }

        [HttpPost, Route("archivos")]
        public async Task<IHttpActionResult> Create(FeiFilesDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var federationFei = await _context.FederationFEIs
                .AsNoTracking().FirstOrDefaultAsync();

            var fei = new FEIFile()
            {
                Nanme = model.Name,                
                FederationFEIId = federationFei.Id
            };

            if (!string.IsNullOrEmpty(model.Base64File))
            {
                string filePath = SaveFile(model.Base64File, model.Extension);
                fei.FileUrl = filePath;
            }
            _context.FEIFiles.Add(fei);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut, Route("archivos/{id}")]
        public async Task<IHttpActionResult> FileUpdate(Guid id, FeiFilesDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fei = await _context.FEIFiles.FindAsync(id);
            fei.Nanme = model.Name;
            if (!string.IsNullOrEmpty(model.Base64File))
            {
                if(!string.IsNullOrEmpty(fei.FileUrl))
                    DeleteFile(fei.FileUrl);
                string filePath = SaveFile(model.Base64File, model.Extension);
                fei.FileUrl = filePath;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete, Route("archivos/{id}")]
        public async Task<IHttpActionResult> FileDelete(Guid id)
        {
            var fei = await _context.FEIFiles.FirstOrDefaultAsync(x => x.Id == id);
            if (fei == null)
                return BadRequest($"Archivo con Id '{id}' no encontrada.");
            if (!string.IsNullOrEmpty(fei.FileUrl))
                DeleteFile(fei.FileUrl);
            _context.FEIFiles.Remove(fei);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
