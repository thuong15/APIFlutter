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
            var data = (from a in _context.Topic.Where(x => x.IsDeleted == false && x.ID < model.Id)
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
            var result = from a in _context.Lesson.Where(x => x.IsDeleted == false)
                         select new
                         {
                             id = a.ID,
                             title = a.Name,
                             img = a.Avatar,
                             sttLesson = "Bài học số 40",
                             totalStar = 2,
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
