using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountAdminController : ControllerBase
	{
		private readonly project4Context _context;

		public AccountAdminController(project4Context context)
		{
			_context = context;
		}




		[HttpGet("phantrangdynamic")]
		public async Task<IActionResult> sapxepdynamic(int page = 1, int pageSize = 2, string? q = "", string? sortBy = "")// ,bool isSortAscending = true
		{

			var Page = page;
			var PageSize = pageSize;


			var query = _context.Account.Where(x=>x.IsDeleted == false && x.IsAdmin == false).AsQueryable();

			if (!string.IsNullOrEmpty(q))//tìm kiếm
			{
				query = query.Where(p => p.Name.ToLower().Contains(q));
			}

			var result = await query
				.Skip(((Page - 1) * PageSize)) // Phân trang
				.Take(pageSize)
				.ToListAsync();
			return Ok(result);
		}

		[HttpPost("GetAccountById")]
		public async Task<IActionResult> GetAccountById([FromBody] Item model)
		{
			var result = _context.Account.Where(t => t.Code == model.Code).ToList();
			return Ok(result);
		}
		[HttpPost("DeleteAdminByCode")]
		public async Task<IActionResult> DeleteTopicById([FromBody] Item model)
		{
			var acc = await _context.Account.FirstOrDefaultAsync(t => t.Code == model.Code);
			if (acc == null)
			{
				return NotFound();
			}
			_context.Account.Remove(acc);
			await _context.SaveChangesAsync();

			return Ok();
		}

	}



}
