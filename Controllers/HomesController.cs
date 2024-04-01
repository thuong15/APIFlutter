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
            var data = from a in _context.Topic.Where(x => !x.IsDeleted && x.Code == model.CodeTopic)
                       join b in _context.Lesson.Where(x => !x.IsDeleted) on a.Code equals b.TopicCode
                       join c in _context.History.Where(x => !x.IsDeleted && x.IsNew) on b.Code equals c.LessonCode into d
                       from c in d.DefaultIfEmpty()
                       select new
                       {
                           id = b.ID,
                           title = b.Name,
                           img = a.Avatar,
                           totalStar = 2,
                           test = c.Status ? "D" : "S",
                           wCode = c.WordCode,
                           qCode = c.QuestionCode
                       };

            var result = from a in _context.Topic.Where(x => !x.IsDeleted && x.Code == model.CodeTopic)
                         join b in _context.Lesson.Where(x => !x.IsDeleted) on a.Code equals b.TopicCode
                         select new
                         {
                             id = b.ID,
                             title = b.Name,
                             img = a.Avatar,
                             totalStar = 2
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
