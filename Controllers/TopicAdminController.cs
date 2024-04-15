using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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

		[HttpPost("AddTopic")]
		public async Task<IActionResult> AddDataTopic(ModeViewTopic modeViewTopic)
		{
			var newTopic = new Topic
			{
				Code = modeViewTopic.Code,
				Name = modeViewTopic.Name,
				Avatar = modeViewTopic.Avatar,
				ComboColor = modeViewTopic.ComboColor,
				CreatedBy = modeViewTopic.CreatedBy,
				CreatedTime = modeViewTopic.CreatedTime,
				DeletedBy = "",
				Description = "",
				UpdatedBy = "",
				IsDeleted = false,
			};

			_context.Topic.Add(newTopic);
			_context.SaveChanges();

			return Ok();

		}

		[HttpPost("UpdateTopic")]
		public async Task<IActionResult> UpdateDataTopicByCode(ModeViewTopic modeViewTopic)
		{
			try
			{
				var check = await _context.Topic.Where(t => t.IsDeleted == false).FirstOrDefaultAsync(t => t.Code == modeViewTopic.Code);
				if (check == null)
				{
					return NotFound();
				}
				check.Name = modeViewTopic.Name;
				check.Avatar = modeViewTopic.Avatar;
				check.ComboColor = modeViewTopic.ComboColor;
				check.UpdatedBy = modeViewTopic.UpdateBy;
				check.UpdatedTime = modeViewTopic.UpdatedTime;

				_context.Topic.Update(check);
				await _context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}

		}

		[HttpPost("DeleteTopicByCode")]
		public async Task<IActionResult> DeleteTopicById([FromBody] Item model)
		{
			var topic = await _context.Topic.FirstOrDefaultAsync(t => t.Code == model.Code);
			if (topic != null)
			{
				topic.IsDeleted = true;
				topic.DeletedTime = DateTime.Now;
				topic.DeletedBy = "admin";
				_context.SaveChanges();
			}
			return Ok();
		}
	}
}
//if (topic == null)
//{
//    return NotFound();
//}
//_context.Topic.Remove(topic);
//await _context.SaveChangesAsync();
