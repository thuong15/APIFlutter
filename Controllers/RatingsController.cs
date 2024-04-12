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
                               join b in _context.History.Where(x => x.IsDeleted == false && x.IsNew == true && x.IsCorrect == true && x.CreatedTime >= startDatePreviousMonth && x.CreatedTime <= endDatePreviousMonth)
                               on a.Code equals b.UserCode
                               select new
                               {
                                   a.Code,
                                   a.Avatar,
                                   a.Name,
                                   b.IsCorrect,
                               }into c
                               group c by new { c.Code } into groupedData
                               select new
                               {
                                   groupedData.Key.Code,
                                   groupedData.First().Name,
                                   groupedData.First().Avatar,
                                   totalscore = groupedData.Count(x => x.IsCorrect),
                               }).OrderByDescending(x => x.totalscore).FirstOrDefault();

            var data = (from a in _context.Account.Where(x => x.IsDeleted == false)
                        join b in _context.History.Where(x => x.IsDeleted == false && x.IsNew == true && x.IsCorrect == true && x.CreatedTime >= startDate && x.CreatedTime <= endDate)
                        on a.Code equals b.UserCode
                        select new
                        {
                            a.Name,
                            a.Avatar,
                            a.Code,
                            b.IsCorrect
                        } into c
                        group c by c.Code into groupedData
                        select new
                        {
                            groupedData.Key,
                            name = groupedData.First().Name,
                            avartar = groupedData.First().Avatar,
                            isUser = groupedData.First().Code == modelViewRetings.UserCode ? true : false,
							totalscore = groupedData.Count(x => x.IsCorrect),
                        }).OrderByDescending(x => x.totalscore);
            var result = new
            {
                dataHistory = dataHistory,
                TotalScoreByAllUsers = data,
            };
            return Ok(result);
        }
    }
}