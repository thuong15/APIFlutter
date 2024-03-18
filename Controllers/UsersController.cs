using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4.model;
using System.Security.Cryptography;
using System.Text;

namespace project4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly project4Context _project4Context;

		public UsersController(project4Context project4Context)
		{
			_project4Context = project4Context;
		}


		public static string GetMd5Hash(string input)
		{
			using (MD5 md5Hash = MD5.Create())
			{
				byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
				StringBuilder builder = new StringBuilder();

				for (int i = 0; i < data.Length; i++)
				{
					builder.Append(data[i].ToString("x2"));
				}

				return builder.ToString();
			}
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(String phone, String password)
		{
			try
			{
				if (!string.IsNullOrEmpty(phone) || !string.IsNullOrEmpty(password))
				{
					string mk = GetMd5Hash(password);

					var newAccount = new Account
					{
						UserName = phone,
						Password = mk,
						IsDeleted = false
					};

					_project4Context.Account.Add(newAccount);
					await _project4Context.SaveChangesAsync();
				}
				else
				{
					return BadRequest("No find");
				}

			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok();
		}
	}
}
