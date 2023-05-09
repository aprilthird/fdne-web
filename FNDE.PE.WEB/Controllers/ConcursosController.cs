using FNDE.PE.WEB.PORTAL.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using FNDE.PE.WEB.PORTAL.ViewModels.ContestViewModels;
using FNDE.PE.WEB.PORTAL.ViewModels.ResultViewModels;
using FDNE.PE.CORE.Helpers;
using ClosedXML.Excel;
using System.IO;

namespace FDNE.PE.WEB.PORTAL.Controllers
{
    [RoutePrefix("concursos")]
    public class ConcursosController : BaseController
    {
        // GET: Concursos
        [Route("resultados")]
        public ActionResult Resultados()
        {
            return View();
        }

        [Route("listadodisciplinasapi")]
        public async Task<ActionResult> ListadoDisciplinasApi()
        {
            var data = await _context.Disciplines.Select( x => new
            {
                id = x.Id,
                name = x.Name
            }).ToListAsync();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Route("listadotemporadasapi")]
        public async Task<ActionResult> ListadoTemporadasApi()
        {
            var result = await _context.Seasons.ToListAsync();

            var data = result.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                year = x.Year
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Route("nivelesxdiciplinasapi/{id}")]
        public async Task<ActionResult> NivelesXDiciplinasApi(Guid id)
        {
            var data = await _context.Levels.Where(x => x.DisciplineId == id).Select(x => new
            {
                id = x.Id,
                name = x.Name,
            }).ToListAsync();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [Route("categoriaxdiciplinasapi/{id}")]
        public async Task<ActionResult> CategoriaXDiciplinasApi(Guid id)
        {
            var data = await _context.Categories.Where(x => x.DisciplineId == id).Select(x => new
            {
                id = x.Id,
                name = x.Name,
            }).ToListAsync();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [Route("listadoeventosapi")]
        public async Task<ActionResult> ListadoEventosApi(Guid disciplineId, Guid seasonId)
        {
            var result = await _context.Events.Where(x => x.DisciplineId == disciplineId && x.SeasonId == seasonId).ToListAsync();
            var data = result.SelectMany(x =>
            {
                if (x.StartTime == x.EndTime)
                    return new[]
                    {
                        new {
                            eventId = x.Id,
                            day = 1,
                            date = x.StartTime.ToDefaultTimeZone(),
                            text = $"{x.StartTime.ToLocalDateFormat()} ({x.Acronym})"
                        }
                    };
                else
                    return Enumerable.Range(0, 1 + x.EndTime.Subtract(x.StartTime).Days)
                        .Select(s => new
                        {
                            eventId = x.Id,
                            day = s + 1,
                            date = x.StartTime.AddDays(s).ToDefaultTimeZone(),
                            text = $"{x.StartTime.AddDays(s).ToLocalDateFormat()} ({x.Acronym})"
                        }).ToArray();
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Route("resultadotoptres")]
        [HttpGet]
        public async Task<ActionResult> ResultadoTopTres(Guid disciplineId, Guid seasonId, Guid eventId, int day, Guid? categoryId = null, Guid? levelId = null)
        {
            try
            {
                var ev = await _context.Events.FindAsync(eventId);
                var date = ev.StartTime.AddDays(day - 1);
                var query = _context.Results
                    .Where(x => x.Ranking.DisciplineId == disciplineId && x.Ranking.SeasonId == seasonId && x.EventId == eventId)
                    .Where(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day)
                    .AsNoTracking();
                if (categoryId.HasValue)
                    query = query.Where(x => x.Ranking.CategoryId == categoryId.Value);
                if (levelId.HasValue)
                    query = query.Where(x => x.Ranking.LevelId == levelId.Value);

                var data = await query
                    .OrderByDescending(x => x.Score)
                    .Take(3)
                    .Select(x => new
                    {
                        rider = x.Rider.User.Name + " " + x.Rider.User.PaternalSurname + " " + x.Rider.User.MaternalSurname,
                        horse = x.Horse.Name,
                        club = x.Club.Name,
                        score = x.Score,
                        percent = x.Percent
                    }).ToListAsync();

                return Json(data, JsonRequestBehavior.AllowGet);
            } catch(Exception e)
            {
                    return Json(new List<ResponseResult>(), JsonRequestBehavior.AllowGet);
            }
        }

        [Route("initpagetoptres")]
        public async Task<ActionResult> InitPageTopTres()
        {
            try
            {
                var discipline = await _context.Disciplines.FirstOrDefaultAsync(x => x.Name == "Salto");
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == "Adulto");
                var sesson = await _context.Seasons.FirstOrDefaultAsync(x => x.Year == DateTime.Now.Year);

                var ranking = await _context.Rankings.FirstOrDefaultAsync(x => x.SeasonId == sesson.Id && x.DisciplineId == discipline.Id && x.LevelId == null && x.SeasonId == sesson.Id && x.CategoryId == category.Id);

                var evnt = await _context.Events.Where(x => x.EndTime < DateTime.Now).OrderByDescending(x => x.EndTime).FirstAsync();

                var data = await _context.Results.Where(x => x.RankingId == ranking.Id && x.EventId == evnt.Id).Select(x => new ResponseResult
                {
                    RiderName = x.Rider.User.Name + " " + x.Rider.User.PaternalSurname + " " + x.Rider.User.MaternalSurname,
                    HorseName = x.Horse.Name,
                    ClunbName = x.Club.Name,
                    Score = x.Score
                }).OrderByDescending(x => x.Score).Take(3).ToListAsync();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new List<ResponseResult>(), JsonRequestBehavior.AllowGet);
            }

        }

        [Route("listadorankingapi")]
        public async Task<JsonResult> ListadoRankingApi(Guid seasonId, Guid disciplineId, Guid? categoryId = null, Guid? levelId = null)
        {
            var query = _context.Binomials.Where(x => x.Ranking.SeasonId == seasonId &&
                x.Ranking.DisciplineId == disciplineId).AsQueryable();
            if (categoryId.HasValue)
                query = query.Where(x => x.Ranking.CategoryId == categoryId).AsQueryable();
            if(levelId.HasValue)
                query = query.Where(x => x.Ranking.LevelId == levelId);
            var data = await query
                .OrderByDescending(x => x.Results.Sum(g => g.Score))
                .Include(x => x.Rider.User).Include(x => x.Horse)
                .Include(x => x.Ranking.Level).Include(x => x.Ranking.Category)
                .Include(x => x.Club).Include(x => x.Results).ToListAsync();
            var model = data
                .Select(x => new
                {
                    rankingId = x.RankingId,
                    rider = x.Rider.User.FullName,
                    riderId = x.RiderId,
                    horse = x.Horse?.Name,
                    horseId = x.HorseId,
                    level = x.Ranking.Level?.Name,
                    category = x.Ranking.Category?.Name,
                    club = x.Club?.Name,
                    clubId = x.ClubId,
                    score = x.Results?.Sum(g => g.Score).ToString("0.00")
                }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Route("ranking/{rankingId}/club/{clubId}/jinete/{riderId}/caballo/{horseId}/detalle")]
        public async Task<ActionResult> Detail(Guid rankingId, Guid clubId, string riderId, Guid horseId)
        {
            var data = await _context.Results
                .Include(x => x.Event.Club)
                .Where(x => x.RankingId == rankingId && x.ClubId == clubId
                    && x.RiderId == riderId && x.HorseId == horseId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
            var model = data.Select(x => new
            {
                @event = x.Event.Name,
                date = x.Date.ToLocalDateFormat(),
                club = x.Event.Club.Name,
                percent = x.Percent,
                plays = x.Plays,
                score = x.Score.ToString("0.00")
            }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Route("rankings/excel")]
        public async Task<ActionResult> DownloadExcel(Guid seasonId, Guid disciplineId, Guid? categoryId = null, Guid? levelId = null)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Ranking");
            var query = _context.Binomials.Where(x => x.Ranking.SeasonId == seasonId &&
                x.Ranking.DisciplineId == disciplineId).AsQueryable();
            if (categoryId.HasValue)
                query = query.Where(x => x.Ranking.CategoryId == categoryId).AsQueryable();
            if (levelId.HasValue)
                query = query.Where(x => x.Ranking.LevelId == levelId);
            var data = await query
                .OrderByDescending(x => x.Results.Sum(g => g.Score))
                .Include(x => x.Rider.User).Include(x => x.Horse)
                .Include(x => x.Ranking.Level).Include(x => x.Ranking.Category)
                .Include(x => x.Club).Include(x => x.Results).ToListAsync();

            worksheet.Cell(1, 1).SetValue("JINETE");
            worksheet.Cell(1, 2).SetValue("CABALLO");
            worksheet.Cell(1, 3).SetValue("NIVEL");
            worksheet.Cell(1, 4).SetValue("CATEGORÍA");
            worksheet.Cell(1, 5).SetValue("CLUB");
            worksheet.Cell(1, 6).SetValue("PUNTAJE");
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.Font.FontColor = XLColor.White;
            worksheet.Row(1).Cells(1, 6).Style.Fill.BackgroundColor = XLColor.DarkRed;

            var rowNumber = 2;
            foreach(var result in data)
            {
                worksheet.Cell(rowNumber, 1).SetValue(result.Rider.User?.FullName);
                worksheet.Cell(rowNumber, 2).SetValue(result.Horse?.Name);
                worksheet.Cell(rowNumber, 3).SetValue(result.Ranking.Level?.Name);
                worksheet.Cell(rowNumber, 4).SetValue(result.Ranking.Category?.Name);
                worksheet.Cell(rowNumber, 5).SetValue(result.Club?.Name);
                worksheet.Cell(rowNumber, 6).SetValue(result.Results?.Sum(g => g.Score).ToString("0.00"));
                rowNumber++;
            }
            worksheet.Columns(1, 6).AdjustToContents();

            var bytes = new byte[] { };
            using (var ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                ms.Position = 0;
                bytes = ms.ToArray();
            }
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheet.sheet", "Ranking.xlsx");
        }

        [Route("rankings")]
        public ActionResult Rankings() => View();

        [Route("fei")]
        public async Task<ActionResult> FEI()
        {
            var model = await _context.FederationFEIs
                .Select(x => new FederationFEIViewModel
                { Body = x.Body,
                    FileUrl = x.FileUrl,
                    ImageUrl = x.ImageUrl,
                    SubTitle = x.SubTitle,
                    Title = x.Title,
                    FeiFiles = _context.FEIFiles.Select(y => new FeiFilesViewModel
                    {
                        Name = y.Nanme,
                        
                    }).ToList()
                }).AsNoTracking().FirstOrDefaultAsync();
            model.ImageUrl = string.IsNullOrEmpty(model.ImageUrl)
                ? Url.Content("~/Content/src/img/slide2-1024.png") : ToImageStorageUrl(model.ImageUrl);
            model.FileUrl = string.IsNullOrEmpty(model.FileUrl)
                ? string.Empty : ToImageStorageUrl(model.FileUrl);
            foreach (var file in model.FeiFiles)
                file.UrlFile = string.IsNullOrEmpty(file.UrlFile)
                    ? string.Empty : ToFileStorageUrl(file.UrlFile);
            return View(model);
        }
    }
}