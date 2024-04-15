using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using project4.model;
using project4.ModelView;
using System.Collections.Generic;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly project4Context _context;

        public RatingsController(project4Context context)
        {
            _context = context;
        }
        [HttpPost("RetingsUser")]
        public async Task<IActionResult> RetingsUser([FromBody] ModelViewRetings modelViewRetings)
        {
            DateTime Date = DateTime.Now;
            DateTime startDate = new DateTime(Date.Year, Date.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            var startDatePreviousMonth = startDate.AddMonths(-1);
            var endDatePreviousMonth = endDate.AddMonths(-1);

            var dataHistory = (from a in _context.Account.Where(x => x.IsDeleted == false)
                               join b in _context.History.Where(x => x.IsDeleted == false && x.IsCorrect == true && x.CreatedTime >= startDatePreviousMonth && x.CreatedTime <= endDatePreviousMonth)
                               on a.Code equals b.UserCode
                               select new
                               {
                                   a.Code,
                                   a.Avatar,
                                   a.Name,
                                   b.WordCode,
                                   b.QuestionCode
                               }
                               into c
                               group c by new { c.WordCode, c.QuestionCode, c.Code } into groupedData
                               select new
                               {
                                   groupedData.Key.Code,
                                   groupedData.First().Name,
                                   groupedData.First().Avatar,
                               } into d
                               group d by d.Code into g
                               select new
                               {
                                   g.Key,
                                   g.First().Name,
                                   g.First().Avatar,
                                   totalscore = g.Count(),
                               }).OrderByDescending(x => x.totalscore)
                               .FirstOrDefault()
                               ;

            var data1 = from b in _context.Lesson.Where(x => !x.IsDeleted)
                               join c in _context.History.Where(x => !x.IsDeleted && x.IsCorrect) on b.Code equals c.LessonCode into k
                               from c in k.DefaultIfEmpty()
                               select new
                               {
                                   code = b.Code,
                                   check = c != null ? true : false,
                                   wCode = c.WordCode,
                                   qCode = c.QuestionCode
                               }
                               into e
                               group e by new { e.code, e.wCode, e.qCode } into h
                               select new
                               {
                                   code = h.Key.code,
                                   check = h.First().check
                               }
                               into d
                               group d by d.code into groupdata
                               select new
                               {
                                   code = groupdata.Key,
                                   Count = !groupdata.First().check ? 0 : groupdata.Count()
                               }
                               ;


            var data = (from a in _context.Account.Where(x => x.IsDeleted == false)
                        join b in _context.History.Where(x => x.IsDeleted == false && x.IsCorrect == true && x.CreatedTime >= startDate && x.CreatedTime <= endDate)
                        on a.Code equals b.UserCode
                        select new
                        {
                            a.Name,
                            a.Avatar,
                            a.Code,
                            b.WordCode,
                            b.QuestionCode,
                            b.IsCorrect
                        } into c
                        group c by new { c.WordCode, c.QuestionCode, c.Code } into groupedData
                        select new
                        {
                            groupedData.Key.Code,
                            name = groupedData.First().Name,
                            avartar = groupedData.First().Avatar,
                            isUser = groupedData.First().Code == modelViewRetings.UserCode ? true : false,
                            totalscore = groupedData.Count(x => x.IsCorrect) >= 1 ? 1 : 0,
                        }into h group h by h.Code into k
                         select new
                         {
                             k.Key,
                             name = k.First().name,
                             avartar = k.First().avartar,
                             isUser = k.First().Code == modelViewRetings.UserCode ? true : false,
                             totalscore = k.Count(),
                         }
                         
                        ).OrderByDescending(x => x.totalscore);
            var result = new
            {
                dataHistory = dataHistory,
                TotalScoreByAllUsers = data,
            };
            return Ok(result);
        }
    }
}