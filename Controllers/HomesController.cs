using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomesController : ControllerBase
    {
        private readonly project4Context _context;

        public HomesController(project4Context context)
        {
            _context = context;
        }

        [HttpPost("GetSttTopic")]
        public async Task<IActionResult> GetSttTopic([FromBody] Item model)
        {
            var topic = _context.Topic.FirstOrDefault(x => !x.IsDeleted && x.Code == model.Code);
            var data = (from a in _context.Topic.Where(x => x.IsDeleted == false && x.ID < topic.ID)
                        select new
                        {
                            Name = a.Name
                        }).ToList();

            var result = new
            {
                stt = data.Count() + 1
            };

            return Ok(result);
        }
        [HttpPost("GetListLesson")]
        public async Task<IActionResult> GetListLesson([FromBody] ItemGetListLesson model)
        {
            var check = from a in _context.Lesson.Where(x => !x.IsDeleted)
                       select new
                       {
                           LessonCode = a.Code,
                           TopicCode = a.TopicCode,
                           Count = _context.Word.Count(b => b.LessonCode == a.Code && !b.IsDeleted) + _context.Question.Count(b => b.LessonCode == a.Code && !b.IsDeleted)
                       };

            var check1 = from a in _context.Topic.Where(x => !x.IsDeleted && x.Code == model.CodeTopic)
                       join b in _context.Lesson.Where(x => !x.IsDeleted) on a.Code equals b.TopicCode
                       join c in _context.History.Where(x => !x.IsDeleted && x.IsNew) on b.Code equals c.LessonCode into d
                       from c in d.DefaultIfEmpty()
                       select new
                       {
                           code = b.Code,
                           count = c.IsCorrect ? 1 : 0,
                           wCode = c.WordCode,
                           qCode = c.QuestionCode
                       } into e
                       group e by e.code into h
                       select new
                       {
                           Code = h.Key,
                           Count = h.Sum(x=>x.count),
                       };

            var data = from a in check
                          join b in check1 on a.LessonCode equals b.Code
                          select new
                          {
                              a.LessonCode,
                              totalStar = b.Count == 0 ? 0 : a.Count == b.Count ? 3 : ((b.Count / a.Count) * 100) > 60 ? 2 : 1,
                          };

            var result = from a in _context.Topic.Where(x => !x.IsDeleted && x.Code == model.CodeTopic)
                         join b in _context.Lesson.Where(x => !x.IsDeleted) on a.Code equals b.TopicCode
                         join c in data on b.Code equals c.LessonCode
                         select new
                         {
                             id = b.ID,
                             code = b.Code,
                             title = b.Name,
                             img = b.Avatar,
                             totalStar = c.totalStar
                         };


            return Ok(result);
        }
        [HttpPost("GetDay")]
        public async Task<IActionResult> GetDay([FromBody] Item model)
        {
            var result = new
            {
                totalDay = '3'
            };

            return Ok(result);
        }
    }
}
