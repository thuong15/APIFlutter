using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListLessonsController : ControllerBase
	{
		private readonly project4Context _context;

		public ListLessonsController(project4Context context)
		{
			_context = context;
		}
		[HttpPost("getListLisson")]
		public async Task<IActionResult> getListLesson([FromBody] ModelViewListLesson modelViewListLesson)
		{
			var data = from a in _context.Lesson.Where(x => x.IsDeleted == false)
					   join b in _context.History.Where(x => x.IsDeleted == false && x.UserCode == modelViewListLesson.codeUser && x.IsNew == true)
					   on a.Code equals b.LessonCode
					   group a by a.Code into lessonGroup
					   select new
					   {
						   LessonCode = lessonGroup.Key,
						   Name = lessonGroup.Select(a => a.Name).FirstOrDefault(),
						   totalquestion = _context.Question.Where(x => x.LessonCode == lessonGroup.Key && x.IsDeleted == false).Count(),
                        };
            var totalCount = data.Count();
			var container = new
			{
				totalCount = totalCount,
                data = data,
			};
            return Ok(container);
		}
	}
}
