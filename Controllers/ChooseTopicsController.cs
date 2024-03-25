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
            var result = from a in _context.Topic.Where(x => x.IsDeleted == false)
                        select new
                        {
                            title = a.Name,
                            img = a.Avatar,
                            comboColor = a.ComboColor,
                            compleLesson = 15,
                            totalLesson = 30
                        };

            return Ok(result);
        }
    }
}
