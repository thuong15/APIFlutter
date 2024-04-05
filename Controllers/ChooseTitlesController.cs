using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChooseTitlesController : ControllerBase
    {
        private readonly project4Context _context;

        public ChooseTitlesController(project4Context context)
        {
            _context = context;
        }
        [HttpPost("GetData")] 
        public async Task<IActionResult> GetData([FromBody] Item model)
        {
            var result = new
            {
                totalWord = _context.Word.Where(x => !x.IsDeleted && x.LessonCode == model.Code).Count(),
                totalQuestion = (from a in _context.Question.Where(x => !x.IsDeleted && x.LessonCode == model.Code)
                                 join b in _context.Answer.Where(x=>!x.IsDeleted) on a.Code equals b.QuestionCode
                                 group a by a.Code into b
                                 select new
                                 {
                                     b.Key
                                 }).Count(),
                totalPuzzle = (from a in _context.Question.Where(x => !x.IsDeleted && x.LessonCode == model.Code)
                               join b in _context.Answer.Where(x => !x.IsDeleted) on a.Code equals b.QuestionCode into c
                               from b in c.DefaultIfEmpty()
                               where b == null
                               group a by a.Code into b
                               select new
                               {
                                   b.Key
                               }).Count(),
            };
            return Ok(result);
        }
    }
}
