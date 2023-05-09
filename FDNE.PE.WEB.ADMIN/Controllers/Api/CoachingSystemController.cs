using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Models;
using FDNE.PE.WEB.ADMIN.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    [RoutePrefix("api/contenido/coaching-system")]
    [Authorize(Roles = ConstantHelpers.ROLE.ADMIN)]
    public class CoachingSystemController : BaseApiController
    {
        public CoachingSystemController() : base()
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var model = await _context.CoachingSystems.AsNoTracking().FirstOrDefaultAsync();
            if (model == null)
            {
                model = new CoachingSystem()
                {
                    FirstTab = "Primer tab",
                    FirstBody = "Primer Cuerpo",
                    SecondTab = "Segundo tab",
                    SecondBody = "Segundo Cuerpo",
                    ThirdTab = "Tercer tab",
                    ThirdBody = "Tercer Cuerpo",
                    QuarterTab = "Cuarto tab",
                    QuarterBody = "Cuarto Cuerpo",
                    FifthTab = "Quinto tab",
                    FifthBody = "Quinto Cuerpo",
                    SixthTab = "Sexto tab",
                    SixthBody = "Sexto Cuerpo"
                };
                _context.CoachingSystems.Add(model);
                await _context.SaveChangesAsync();
            }
            var result = new CoachingSystemDto()
            {
                FirstTab = model.FirstTab,
                FirstBody = model.FirstBody,
                SecondTab = model.SecondTab,
                SecondBody = model.SecondBody,
                ThirdTab = model.ThirdTab,
                ThirdBody = model.ThirdBody,
                QuarterTab = model.QuarterTab,
                QuarterBody = model.QuarterBody,
                FifthTab = model.FifthTab,
                FifthBody = model.FifthBody,
                SixthTab = model.SixthTab,
                SixthBody = model.SixthBody
            };
            return Ok(result);
        }

        [HttpPut, Route("")]
        public async Task<IHttpActionResult> Update(CoachingSystemDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var coachingSystem = await _context.CoachingSystems.FirstOrDefaultAsync();

            coachingSystem.FirstTab = model.FirstTab;
            coachingSystem.FirstBody = model.FirstBody;
            coachingSystem.SecondTab = model.SecondTab;
            coachingSystem.SecondBody = model.SecondBody;
            coachingSystem.ThirdTab = model.ThirdTab;
            coachingSystem.ThirdBody = model.ThirdBody;
            coachingSystem.QuarterTab = model.QuarterTab;
            coachingSystem.QuarterBody = model.QuarterBody;
            coachingSystem.FifthTab = model.FifthTab;
            coachingSystem.FifthBody = model.FifthBody;
            coachingSystem.SixthTab = model.SixthTab;
            coachingSystem.SixthBody = model.SixthBody;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
