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
    [RoutePrefix("api/federation-historia")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class FederationHistoryController : BaseApiController
    {
        public FederationHistoryController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var federationHistory = await _context.FederationHistories
                .AsNoTracking().FirstOrDefaultAsync();

            if (federationHistory == null)
            {
                federationHistory = new FederationHistory()
                {
                    UrlImage = ""
                };
                _context.FederationHistories.Add(federationHistory);
                await _context.SaveChangesAsync();
            }

            var result = new FederationHistoryDto
            {
                Id = federationHistory.Id,
                ImageUrl = federationHistory.UrlImage
            };
            return Ok(result);

        }

        [HttpPut, Route("{id}")]
        public async Task<IHttpActionResult> Update(FederationHistoryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var federationHistory = await _context.FederationHistories.FindAsync(model.Id);

            if (!string.IsNullOrEmpty(model.Base64Image))
            {
                if (!string.IsNullOrEmpty(federationHistory.UrlImage))
                {
                    DeleteImage(federationHistory.UrlImage);
                }
                string pathImage = SaveImage(model.Base64Image, ConstantHelpers.IMAGEFOLDER.GENERAL);
                federationHistory.UrlImage = pathImage;
            }

            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpGet, Route("periodos")]
        public async Task<IHttpActionResult> GetOfficer()
        {
            var federationHistory = await _context.FederationHistories
                                     .AsNoTracking().FirstOrDefaultAsync();
            if (!_context.HistoryPerYears.Any())
            {
                for (int i = 0; i < 4; i++)
                {
                    var history = new HistoryPerYear()
                    {
                        Year = 1900,
                        Body = "Texto de Prueba",
                        FederationHistoryId = federationHistory.Id
                    };
                    _context.HistoryPerYears.Add(history);
                    await _context.SaveChangesAsync();
                }
            }

            var data = await _context.HistoryPerYears.AsNoTracking().ToListAsync();

            var result = data.Select(x => new HistoryPerYearDto
            {
                Id = x.Id,
                Year = x.Year,
                Body = x.Body,
                FederationHistoryId = x.FederationHistoryId
            });

            return Ok(result);
        }

        [HttpGet, Route("periodos/{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var data = await _context.HistoryPerYears.FindAsync(id);
            var result = new HistoryPerYearDto()
            {
                Id = data.Id,
                Year = data.Year,
                Body = data.Body,
                FederationHistoryId = data.FederationHistoryId
            };
            return Ok(result);
        }

        [HttpPut, Route("periodos/{id}")]
        public async Task<IHttpActionResult> YearUpdate(Guid id, HistoryPerYearDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var history = await _context.HistoryPerYears.FindAsync(id);
            history.Year = model.Year;
            history.Body = model.Body;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
