using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProFileController : ControllerBase
	{
		private readonly project4Context _context;

		public ProFileController(project4Context context)
		{
			_context = context;
		}
		[HttpPost("ProFile")]
		public async Task<IActionResult> ProFile([FromBody] ModelViewProFile model)
		{
			DateTime Date = DateTime.Now;
			DateTime startDate = new DateTime(Date.Year, Date.Month, 1);
			DateTime endDate = startDate.AddMonths(1).AddDays(-1);
			var dataUser = _context.Account.Where(x => x.IsDeleted == false && x.Code == model.codeUser).Select(x => new
			{
				x.Name,
				x.UserName,
				x.Avatar,
				x.Code,
				pass = x.Password,
				createdtime = x.CreatedTime.Value.ToString("dd/MM/yyyy"),
			}).FirstOrDefault();
			var totalscoreQuery = (from a in _context.Account.Where(x => x.IsDeleted == false && x.Code == model.codeUser)
								   join b in _context.History.Where(x => x.IsDeleted == false && x.IsCorrect == true && x.CreatedTime >= startDate && x.CreatedTime <= endDate)
								   on a.Code equals b.UserCode
								   group b by new { b.WordCode, b.QuestionCode, b.UserCode } into groupedData
								   select new
								   {
									   groupedData.Key.UserCode,
									   totalscore = groupedData.Count(x => x.IsCorrect) >= 1 ? 1 : 0,
								   } into c
								   group c by c.UserCode into groupedData
								   select new
								   {
									   groupedData.Key,
									   totalscore = groupedData.Count(),
								   }
								   ).FirstOrDefault();
			int totalscore = totalscoreQuery != null ? totalscoreQuery.totalscore : 0;
			var data = (from a in _context.Account.Where(x => x.IsDeleted == false)
						join b in _context.History.Where(x => x.IsDeleted == false && x.IsNew == true && x.IsCorrect == true && x.CreatedTime >= startDate && x.CreatedTime <= endDate)
						on a.Code equals b.UserCode
						select new
						{
							a.Name,
							a.Avatar,
							a.Code,
							b.IsCorrect
						} into c
						group c by c.Code into groupedData
						select new
						{
							groupedData.Key,
							name = groupedData.First().Name,
							avartar = groupedData.First().Avatar,
							totalscore = groupedData.Count(x => x.IsCorrect),
						}).OrderByDescending(x => x.totalscore);
			var ratingsUser = data.ToList().FindIndex(x => x.Key == model.codeUser) + 1;
			var result = new
			{
				dataUser = dataUser,
				totalscore = totalscore,
				userPosition = ratingsUser,
			};
			return Ok(result);
		}
		[HttpPost("EditProFileName")]
		public async Task<IActionResult> EditProFileName([FromBody] EditProFile edit)
		{
			bool status = false;
			String title = "";
			try
			{
				var data = _context.Account.FirstOrDefault(x => x.IsDeleted == false && x.Code == edit.codeUser);
				var dataCheck = _context.Account.FirstOrDefault(x => x.IsDeleted == false && x.Name == edit.name);
				if (data != null && dataCheck == null)
				{
					data.Name = edit.name;
					data.UpdatedBy = "admin";
					data.UpdatedTime = DateTime.Now;
					_context.Account.Update(data);
					_context.SaveChanges();
					status = true;
					title = "Đổi tên thành công.";
				}
				else
				{
					status = false;
					title = data == null ? "Không tìm thấy tài khoản của bạn." : "Tên người dùng đã tồn tại!";
				}
			}
			catch (Exception ex)
			{
				status = false;
				title = "Lỗi không đổi được tên vui lòng thử lại lần sau.";
			}
			var result = new
			{
				status = status,
				title = title,
			};
			return Ok(result);
		}
		[HttpPost("EditAvatar")]
		public async Task<IActionResult> EditAvatar([FromBody] EditAvatar edit)
		{
			bool status = false;
			String title = "";
			try
			{
				var data = _context.Account.FirstOrDefault(x => x.IsDeleted == false && x.Code == edit.codeUser);
				if (data != null)
				{
					data.Avatar = edit.avatar;
					data.UpdatedBy = "admin";
					data.UpdatedTime = DateTime.Now;
					_context.Account.Update(data);
					_context.SaveChanges();
					status = true;
					title = "Đổi ảnh đại diện thành công.";
				}
				else
				{
					status = false;
					title = "Không tìm thấy tài khoản của bạn.";
				}
			}
			catch (Exception ex)
			{
				status = false;
				title = "Lỗi không đổi được ảnh đại diện vui lòng thử lại lần sau.";
			}
			var result = new
			{
				status = status,
				title = title,
			};
			return Ok(result);
		}

		[HttpPost("EditProFilePass")]
		public async Task<IActionResult> EditProFilePass([FromBody] EditProFile edit)
		{
			bool status = false;
			String title = "";
			try
			{
				var passnew = edit.password;
				using (MD5 md5 = MD5.Create())
				{
					// Chuyển đổi chuỗi thành mảng byte và tính toán giá trị băm (hash)
					byte[] inputBytes = Encoding.ASCII.GetBytes(passnew);
					byte[] hashBytes = md5.ComputeHash(inputBytes);

					// Chuyển đổi mảng byte thành chuỗi hex
					StringBuilder sb = new StringBuilder();
					for (int i = 0; i < hashBytes.Length; i++)
					{
						sb.Append(hashBytes[i].ToString("x2"));
					}
					passnew = sb.ToString();
				}
				var data = _context.Account.FirstOrDefault(x => x.IsDeleted == false && x.Code == edit.codeUser);
				if (data != null)
				{
					data.Password = passnew;
					data.UpdatedBy = edit.name;
					data.UpdatedTime = DateTime.Now;
					_context.Account.Update(data);
					_context.SaveChanges();
					status = true;
					title = "Đổi mật khẩu thành công.";
				}
				else
				{
					status = false;
					title = "Không tìm thấy tài khoản của bạn.";
				}
			}
			catch (Exception ex)
			{
				status = false;
				title = "Lỗi không đổi được mật khẩu vui lòng thử lại lần sau.";
			}
			var result = new
			{
				status = status,
				title = title,
			};
			return Ok(result);
		}

		[HttpPost("GetListAvatar")]
		public async Task<IActionResult> GetListAvatar([FromBody] Item model)
		{
			var data = from a in _context.Avatar.Where(x => !x.IsDeleted)
					   join b in _context.Account.Where(x => !x.IsDeleted && x.Code == model.Code) on a.Name equals b.Avatar into c
					   from d in c.DefaultIfEmpty()
					   where d == null
					   select new
					   {
						   a.Code,
						   a.Name,
						   isChoose = false
					   };
			return Ok(data);
		}
	}
}

