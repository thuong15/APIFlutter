using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

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
        [HttpPost("GetDetailLesson")]
        public async Task<IActionResult> GetDetailLesson([FromBody] ModelViewDetailLesson modelViewDetailLesson)
        {
            var title = _context.Lesson.FirstOrDefault(x => x.IsDeleted == false && x.Code == modelViewDetailLesson.LessonCode).Name;

            var data = _context.Question.Where(x => x.IsDeleted == false && x.LessonCode == modelViewDetailLesson.LessonCode).Select(x => new
            {

                question = x.Name,
                avatar = x.Avatar,
                listAnswer = _context.Answer.Where(h => h.IsDeleted == false && h.QuestionCode == x.Code).Select(x => new
                {
                    answer = x.Name,
                    isTrue = x.IsTrue,
                }).ToList()
            });
            var total = _context.Question.Where(x => x.IsDeleted == false && x.LessonCode == modelViewDetailLesson.LessonCode).Count();
            var listData = new
            {
                title = title,
                data = data,
                total= total
            };
            return Ok(listData);
        }
    }
}
