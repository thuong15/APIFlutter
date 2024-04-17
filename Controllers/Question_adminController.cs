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


		[HttpPost("GetAllqwa")]
		public async Task<IActionResult> GetQuestionsAndAnswers([FromBody] Item model)
		{

			var data = (from a in _context.Question.Where(x => !x.IsDeleted && x.LessonCode == model.Code)
						join b in _context.Answer.Where(x => !x.IsDeleted) on a.Code equals b.QuestionCode into e
						from b in e.DefaultIfEmpty()
						select new
						{
							QuestionCode = a.Code,
							QuestionName = a.Name,
							IsAnsquestion = b != null,
							orderByDecending = b != null ? 1 : 0,
							AnswerName = b != null ? b.Name : "",
							AnswerCode = b != null ? b.Code : "",
							IsTrue = b != null ? b.IsTrue : false,
						} into c
						group c by c.QuestionCode into d
						select new
						{
							QuestionCode = d.Key,
							QuestionName = d.First().QuestionName,
							order = d.First().orderByDecending,
							Answers = d.Select(x => new { x.AnswerCode, x.AnswerName, x.IsTrue })
						}).OrderByDescending(x=>x.order);
			var result = new
			{
				answerQ = data,
			};

			return Ok(result);
		}
	}
}
