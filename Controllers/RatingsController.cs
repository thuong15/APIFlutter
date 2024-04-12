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

            var user = (from a in _context.Account
                        join b in _context.History
                        on a.Code equals b.UserCode
                        where a.IsDeleted == false
                              && a.Code == modelViewRetings.UserCode
                              && b.IsDeleted == false
                              && b.IsNew == true
                              && b.IsCorrect == true
                              && b.CreatedTime >= startDate && b.CreatedTime <= endDate
                        group a by b.UserCode into groupedData
                        select new
                        {
                            totalscore = groupedData.Count(),
                            name = groupedData.Select(x => x.Name).FirstOrDefault(),
                            avatar = groupedData.Select(x => x.Avatar).FirstOrDefault(),
                        }).FirstOrDefault();

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
            var userPosition = data.ToList().FindIndex(x => x.Key == modelViewRetings.UserCode) + 1;
            var result = new
            {
                TotalScoreByUser = user,
                TotalScoreByAllUsers = data,
                Reting = userPosition,
            };
            return Ok(result);
        }
        [HttpPost("RankLastMonth")]
        public async Task<IActionResult> RankLastMonth()
        {
            DateTime Date = DateTime.Now;
            DateTime startDate = new DateTime(Date.Year, Date.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            var startDatePreviousMonth = startDate.AddMonths(-1);
            var endDatePreviousMonth = endDate.AddMonths(-1);

            var dataHistory = (from a in _context.Account.Where(x => x.IsDeleted == false)
                               join b in _context.History.Where(x => x.IsDeleted == false && x.IsNew == true && x.IsCorrect == true && x.CreatedTime >= startDatePreviousMonth && x.CreatedTime <= endDatePreviousMonth)
                               on a.Code equals b.UserCode
                               group b by new { b.UserCode } into groupedData
                               select new
                               {
                                   groupedData.Key.UserCode,
                                   totalscore = groupedData.Count(x => x.IsCorrect),
                               }).OrderByDescending(x => x.totalscore).FirstOrDefault();
            return Ok(dataHistory);
        }

    }
}