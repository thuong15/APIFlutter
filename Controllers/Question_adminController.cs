using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Question_adminController : ControllerBase
	{
		private readonly project4Context _context;
		public Question_adminController(project4Context context)
		{
			_context = context;
		}


		[HttpGet("GetAllqwa")]
		public async Task<IActionResult> GetQuestionsAndAnswers()
		{
			var questionsWithAnswers = await _context.Question.Where(t => t.IsDeleted == false).Select(question => new
			{
				QuestionCode = question.Code,
				QuestionName = question.Name,
				Answers = _context.Answer.Where(answer => answer.QuestionCode == question.Code && answer.IsDeleted == false).Select(answer => new
				{
					AnswerCode = answer.Code,
					AnswerName = answer.Name,
					IsTrue = answer.IsTrue
				}).ToList()
			}).ToListAsync();

			return Ok(questionsWithAnswers);
		}
	}
}
