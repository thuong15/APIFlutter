using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticalController : ControllerBase
    {
        private readonly project4Context _context;

        public StatisticalController(project4Context context)
        {
            _context = context;
        }

        [HttpPost("GetInfoData")]
        public async Task<IActionResult> GetInfoData([FromBody] Item model)
        {
            //select list word
            var checkListWord = (from a in _context.History.Where(x => !x.IsDeleted && x.UserCode == model.Code && x.WordCode != "")
                                 join b in _context.Word.Where(x => !x.IsDeleted) on a.WordCode equals b.Code
                                 select new
                                 {
                                     a.WordCode,
                                     b.NameEN
                                 } into c
                                 group c by c.WordCode into d
                                 select new
                                 {
                                     d.Key,
                                     NameEN = char.ToUpper(d.First().NameEN.FirstOrDefault()) + d.First().NameEN.Substring(1)
                                 }).ToList();

            string listWordString = "";

            foreach (var item in checkListWord.Take(4))
            {
                listWordString += item.NameEN + ", ";
            }

            //select list question
            var listQuestion = (from a in _context.History.Where(x => !x.IsDeleted && x.UserCode == model.Code && x.QuestionCode != "")
                                join b in _context.Question.Where(x=>!x.IsDeleted) on a.QuestionCode equals b.Code
                                select new
                                {
                                    a.QuestionCode,
                                    b.Name
                                } into c
                                group c by c.QuestionCode into d
                                select new
                                {
                                    d.Key,
                                    d.First().Name
                                }).ToList();

            string listQuestionString = "";

            foreach (var item in listQuestion.Take(4))
            {
                listQuestionString += item.Name.Substring(0, 8) + ", ";
            }

            // select time
            var checkTime = (from a in _context.History.Where(x => !x.IsDeleted && x.CreatedTime.Value.Date == DateTime.Today && x.UserCode == model.Code)
                             select new
                             {
                                 a.CreatedTime
                             }).ToList();
            DateTime? startTime = checkTime.Count() > 0 ? checkTime[0].CreatedTime : DateTime.Today;
            DateTime? endTime = checkTime.Count() > 0 ? checkTime[checkTime.Count() - 1].CreatedTime : DateTime.Today;
            TimeSpan? timeDifference = endTime - startTime;

            String time = "...";

            int? totalDaysDifference = timeDifference.HasValue ? Convert.ToInt32(Math.Abs(timeDifference.Value.TotalDays)) : (int?)null;
            int? totalHoursDifference = timeDifference.HasValue ? Convert.ToInt32(Math.Abs(timeDifference.Value.TotalHours)) : (int?)null;
            int? totalMinutesDifference = timeDifference.HasValue ? Convert.ToInt32(Math.Abs(timeDifference.Value.TotalMinutes)) : (int?)null;
            int? totalSecondsDifference = timeDifference.HasValue ? Convert.ToInt32(Math.Abs(timeDifference.Value.TotalSeconds)) : (int?)null;

            if (totalDaysDifference > 0)
            {
                time = totalDaysDifference + " ngày";
            }
            else if (totalHoursDifference > 0)
            {
                time = totalHoursDifference + " tiếng";
            }
            else if (totalMinutesDifference > 0)
            {
                time = totalMinutesDifference + " phút";
            }
            else if (totalSecondsDifference > 0)
            {
                time = totalSecondsDifference + " giây";
            }

            //sumary variable
            var result = new
            {
                totalWord = checkListWord.Count(),
                totalQuestion = listQuestion.Count(),
                totalNewWord = (from a in _context.History.Where(x => !x.IsDeleted && x.CreatedTime.Value.Date == DateTime.Today && x.UserCode == model.Code && x.WordCode != "")
                                group a by a.WordCode into b
                                select new
                                {
                                    b.Key
                                }).Count(),
                totalNewQues = (from a in _context.History.Where(x => !x.IsDeleted && x.CreatedTime.Value.Date == DateTime.Today && x.UserCode == model.Code && x.QuestionCode != "")
                                group a by a.QuestionCode into b
                                select new
                                {
                                    b.Key
                                }).Count(),
                totalOldWord = (from a in _context.History.Where(x => !x.IsDeleted && x.CreatedTime.Value.Date == DateTime.Today && x.UserCode == model.Code && x.WordCode != "" && x.IsNew)
                                join b in _context.History.Where(x => !x.IsDeleted && x.CreatedTime.Value.Date != DateTime.Today && x.UserCode == model.Code && x.WordCode != "" && !x.IsNew) on a.WordCode equals b.WordCode
                                group a by a.WordCode into c
                                select new
                                {
                                    c.Key
                                }).Count(),
                time = time,
                listWordString = listWordString,
                listQuestionString = listQuestionString
            };

            return Ok(result);
        }
    }
}
