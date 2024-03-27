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
                totalWord = 10,
                totalQuestion = 10,
                totalPuzzle = 10,
            };
            return Ok(result);
        }
    }
}
