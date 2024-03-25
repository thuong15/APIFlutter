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

            //if (data == null || !data.Any())
            //{
            //    return NotFound();
            //}

            var result = new
            {
                stt = data.Count() + 1
            };

            return Ok(result);
        }

    }
}
