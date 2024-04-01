using Microsoft.AspNetCore.Mvc;
using project4.model;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LearnWordController : ControllerBase
	{
		private readonly project4Context _context;

		public LearnWordController(project4Context context)
		{
			_context = context;
		}
		[HttpPost("getDataQuestion")]
		public async Task<IActionResult> getDataQuestion()
		{
			var data = _context.Word
				.Where(x => x.IsDeleted == false)
				.Select(x => new
				{
					x.Avatar,
					x.NameEN,
					x.NameVN,
				}).FirstOrDefault();
			return Ok(data);
		}


		[HttpPost("getDataAnwer")]
		public async Task<IActionResult> getDataAnwer()
		{
			//var data = _context.Word.Where(x=>x.IsDeleted == false)
			return Ok();
		}
	}
}
