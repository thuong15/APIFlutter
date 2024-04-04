using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TopicAdminController : ControllerBase
	{
		
		private readonly project4Context _context;

		public TopicAdminController(project4Context context)
		{
			_context = context;
		}

		[HttpGet("GetTopic")]
		public async Task<IActionResult> GetTopic()
		{
			var result = _context.Topic.Where(x => x.IsDeleted == false).Select(x => new
			{
				ID = x.ID,
				Code = x.Code,
				Name = x.Name,
				Avatar = x.Avatar
			});

			return Ok(result);
		}

		[HttpPost("GetTopicById")]
		public async Task<IActionResult> GetTopicById([FromBody] Item model)
		{
			var result = _context.Topic.Where(x => x.Code == model.Code).Select(x => new
			{
				ID = x.ID,
				Name = x.Name,
				Avatar = x.Avatar,
				Code = x.Code,
				ComboColor = x.ComboColor,
				CreateTime = x.CreatedTime,

			});

			return Ok(result);
		}
	}
}
