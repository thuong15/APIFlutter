using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;

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
		public async Task<IActionResult> getListLesson()
		{
			var data = from a in _context.Lesson.Where(x => x.IsDeleted == false)
						join b in _context.History.Where(x => x.IsDeleted == false && x.UserCode == "viet_1")
						on a.Code equals b.LessonCode
						group a by a.Code into lessonGroup
						select new
						{
							LessonCode = lessonGroup.Key,
							Total = lessonGroup.Count(),
							Name = lessonGroup.Select(a => a.Name).FirstOrDefault(),
						};

			return Ok(data);
		}
	}
}
