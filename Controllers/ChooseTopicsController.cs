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
