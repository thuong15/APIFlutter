using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;
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
		public async Task<IActionResult> GetDataQuestion([FromBody] ModelViewDataAnswer item)
		{
			var data = from a in _context.Lesson.Where(x => x.IsDeleted == false && x.Code == item.CodeLesson)
					   join b in _context.Question.Where(x => x.IsDeleted == false) on a.Code equals b.LessonCode
					   select new
					   {
						   code = b.Code,
						   question = b.Name,
						   avatar = b.Avatar,
					   };
			return Ok(data);
		}
		[HttpPost("GetDataAnswer")]
		public async Task<IActionResult> GetDataAnswer([FromBody] ModelViewDataAnswer item)
		{
			var Listanswer = (from a in _context.Lesson.Where(x => x.IsDeleted == false && x.Code == item.CodeLesson)
							  join b in _context.Question.Where(x => x.IsDeleted == false) on a.Code equals b.LessonCode
							  join c in _context.Answer.Where(x => x.IsDeleted == false) on b.Code equals c.QuestionCode
							  group c by c.QuestionCode into grouped
							  select new
							  {
								  Answers = grouped.Select(x => new { x.Name, x.IsTrue, IsChoose = false }).ToList()
							  }).ToList();
			return Ok(Listanswer);
		}

		[HttpPost("AddHistory")]
		public async Task<IActionResult> AddHistory([FromBody] ModelViewAnswerTheQuestion item)
		{
			try
			{
				var check = _context.History.Where(x => x.IsDeleted == false).OrderByDescending(x => x.ID).FirstOrDefault();

				int id_max = check != null ? check.ID + 1 : 1;

				string dataFlutter = item.CodeLesson;
				string[] parts = dataFlutter.Split('_');
				string CodeLesson = parts[1];

				History itemm = new History()
				{
					Code = "H_" + CodeLesson + "_" + id_max,
					LessonCode = item.CodeLesson,
					IsCorrect = item.IsCorrect,
					UserCode = item.UserCode,
					WordCode = "",
					QuestionCode = item.CodeQuestion,
					IsNew = true,
					CreatedBy = "Admin",
					UpdatedBy = "",
					DeletedBy="",
					CreatedTime = DateTime.Now,
				};

				_context.History.Add(itemm);

				var dataHistory = _context.History.OrderByDescending(x=>x.ID).FirstOrDefault(x => x.IsDeleted == false && x.LessonCode == item.CodeLesson && x.QuestionCode == item.CodeQuestion);
				if (dataHistory!=null)
				{
					dataHistory.IsNew = false;
				}
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return Ok();
		}

	}
}
