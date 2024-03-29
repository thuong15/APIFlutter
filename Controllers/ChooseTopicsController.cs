using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChooseTopicsController : ControllerBase
    {
        private readonly project4Context _context;

        public ChooseTopicsController(project4Context context)
        {
            _context = context;
        }
        [HttpPost("GetListTopic")]
        public async Task<IActionResult> GetListTopic([FromBody] Item model)
        {
            var result10 = from a in _context.Lesson.Where(x => !x.IsDeleted)
                          join b in _context.Word.Where(x => !x.IsDeleted) on a.Code equals b.LessonCode into words
                          join c in _context.Question.Where(x => !x.IsDeleted) on a.Code equals c.LessonCode into questions
                          join d in _context.History.Where(x => !x.IsDeleted) on a.Code equals d.LessonCode into history
                          select new
                          {
                              TopicCode = a.TopicCode,
                              LessonCode = a.Code,
                              isCompleted = words.Count() + questions.Count() == history.Count()
                          };

            var test = from a in _context.Lesson.Where(x => !x.IsDeleted)
                       select new
                       {
                           LessonCode = a.Code,
                           TopicCode = a.TopicCode,
                           Count = _context.Word.Count(b => b.LessonCode == a.Code && !b.IsDeleted) + _context.Question.Count(b => b.LessonCode == a.Code && !b.IsDeleted)
                       };

            var test1 = from a in _context.History.Where(x => !x.IsDeleted)
                        group a by a.LessonCode into g
                        select new
                        {
                            LessonCode = g.Key,
                            Count = g.Count(),
                        };

            var result1 = from t in test
                          join h in test1 on t.LessonCode equals h.LessonCode
                          select new
                          {
                              TopicCode = t.TopicCode,
                              LessonCode = t.LessonCode,
                              isCompleted = t.Count == h.Count
                          };

            var result = from a in _context.Topic.Where(x => !x.IsDeleted)
                         join b in _context.Lesson.Where(x => !x.IsDeleted) on a.Code equals b.TopicCode
                         group a by a.ID into g
                         select new
                         {
                             id = g.Key,
                             title = g.First().Name,
                             img = g.First().Avatar,
                             comboColor = g.First().ComboColor,
                             compleLesson = 1,
                             totalLesson = g.Count()
                         };

            return Ok(result);
        }
    }
}
