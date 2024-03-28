using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListVocabularysController : ControllerBase
    {
        private readonly project4Context _context;

        public ListVocabularysController(project4Context context)
        {
            _context = context;
        }

        [HttpPost("ListWords")]
        public async Task<IActionResult> ListWords([FromBody] ModelItem modelItem)
        {
            var data = _context.Word.Where(x => x.IsDeleted == false).Select(x => new
            {
                x.NameVN,
                x.ID,
                x.NameEN,
				wordCount = 5,
				wordTotalCount =5,
			});
			return Ok(data);
        }
        [HttpGet("Totalnumber")]
        public async Task<IActionResult> Totalnumber()
        {
            var result = new
            {
                totalNumber = 40,
				wordsLearned = 30,
				wordsBeingStudied = 10,
			};
			return Ok(result);
        }
	}
}
