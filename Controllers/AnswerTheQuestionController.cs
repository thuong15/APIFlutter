using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using System.Runtime.InteropServices;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AnswerTheQuestionController : ControllerBase
	{
		private readonly project4Context _context;

		public AnswerTheQuestionController(project4Context context)
		{
			_context = context;
		}
		[HttpPost("GetDataQuestion")]
		public async Task<IActionResult> GetDataQuestion()
		{
			var data = _context.Question.Where(x => x.IsDeleted == false).Select(x => new
			{
				question = x.Name,
				avatar = x.Avatar,
			});
			return Ok(data);
		}
		[HttpPost("GetDataAnswer")]
		public async Task<IActionResult> GetDataAnswer()
		{

			var Listanswer = _context.Answer.Where(z => z.IsDeleted == false && z.QuestionId == 1)
											.Select(x => new { answer = x.Name, x.IsTrue });
			return Ok(Listanswer);
		}
	}
}
