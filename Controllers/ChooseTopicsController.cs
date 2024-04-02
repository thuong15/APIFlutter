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
            var test = from a in _context.Lesson.Where(x => !x.IsDeleted)
                       select new
                       {
                           LessonCode = a.Code,
                           TopicCode = a.TopicCode,
                           Count = _context.Word.Count(b => b.LessonCode == a.Code && !b.IsDeleted) + _context.Question.Count(b => b.LessonCode == a.Code && !b.IsDeleted)
                       };

            var test1 = from a in _context.Lesson.Where(x => !x.IsDeleted)
                        join b in _context.History.Where(x => !x.IsDeleted && x.IsNew) on a.Code equals b.LessonCode into c
                        from b in c.DefaultIfEmpty()
                        select new
                        {
                            a.Code,
                            count = b != null ? 1 : 0
                        } into d
                        group d by d.Code into h
                        select new
                        {
                            LessonCode = h.Key,
                            Count = h.Sum(item => item.count)
                        };

            var result1 = from t in test
                          join h in test1 on t.LessonCode equals h.LessonCode
                          select new
                          {
                              TopicCode = t.TopicCode,
                              LessonCode = t.LessonCode,    
                              count = h.Count != 0 ? t.Count == h.Count ? 1 : 0 : 0
                          }
                          into c
                          group c by c.TopicCode into g
                          select new
                          {
                              TopicCode = g.Key,
                              LessonCode = g.Count(),
                              Count = g.Sum(item => item.count)
                          };

            var result = (from a in _context.Topic.Where(x => !x.IsDeleted)
                          join b in _context.Lesson.Where(x => !x.IsDeleted) on a.Code equals b.TopicCode
                          group a by a.ID into g
                          select new
                          {
                              id = g.Key,
                              codeTopic = g.First().Code,
                              title = g.First().Name,
                              topicCode = g.First().Code,
                              img = g.First().Avatar,
                              comboColor = g.First().ComboColor,
                              totalLesson = g.Count()
                          } into c
                          join d in result1 on c.topicCode equals d.TopicCode
                          select new
                          {
                              id = c.id,
                              title = c.title,
                              codeTopic = c.codeTopic,
                              img = c.img,
                              comboColor = c.comboColor,
                              totalLesson = c.totalLesson,
                              compleLesson = d.Count
                          }).OrderBy(c => c.id);


            return Ok(result);
        }
    }
}
