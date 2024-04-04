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
	public class LessonAdminController : ControllerBase
	{
		private readonly project4Context _context;

		public LessonAdminController(project4Context context)
		{
			_context = context;
		}

		[HttpGet("GetWord")]
		public async Task<IActionResult> GetWord()
		{
			var result = _context.Word.Where(w => w.IsDeleted == false).Select(w => new
			{
				ID = w.ID,
				NameVN = w.NameVN,
				NameEN = w.NameEN,
				Count = w.Count,
				CreateTime = w.CreatedTime,

			});
			return Ok(result);
		}
	}
}
