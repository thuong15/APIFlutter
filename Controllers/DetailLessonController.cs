using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailLessonController : ControllerBase
    {
        private readonly project4Context _context;

        public DetailLessonController(project4Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetDetailLesson()
        {
            var data = _context.Question.Where(x => x.IsDeleted == false && x.LessonCode == "L_GD_1").Select(x => new
            {
                question = x.Name,
                avatar = x.Avatar,
            });
            return Ok();
        }
    }
}
