using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;
using System.Security.Cryptography;
using System.Text;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly project4Context _context;

        public LoginController(project4Context context)
        {
            _context = context;
        }
        [HttpPost("CheckAcc")]
        public async Task<IActionResult> CheckAcc([FromBody] ModelViewLogin modelViewLogin)
        {
            string inputPassword = modelViewLogin.Password;
            bool login = false;
            bool isAdmin = false;
            string code = "";

            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi mật khẩu nhập vào thành giá trị băm
                byte[] inputBytes = Encoding.ASCII.GetBytes(inputPassword);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi giá trị băm thành chuỗi hex
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                string inputHash = sb.ToString();

                var checkUser = _context.Account.FirstOrDefault(x => x.IsDeleted == false
                                                    && x.UserName == modelViewLogin.UserName
                                                    && x.Password == inputHash);
                if (checkUser != null)
                {
                    isAdmin = checkUser.IsAdmin;
                    login = true;
                    code = checkUser.Code;
				}
                else
                {
                    login = false;
                }
            }
            var result = new
            {
                status = login,
                code = code
			};
            return Ok(result);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] DataRegister dataRegister)
        {
            bool status = false;
            string title = "";
            try
            {
                var pass = dataRegister.Password;
                using (MD5 md5 = MD5.Create())
                {
                    // Chuyển đổi chuỗi thành mảng byte và tính toán giá trị băm (hash)
                    byte[] inputBytes = Encoding.ASCII.GetBytes(pass);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Chuyển đổi mảng byte thành chuỗi hex
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("x2"));
                    }
                    pass = sb.ToString();
                }
                var check = _context.Account.OrderByDescending(x => x.ID).FirstOrDefault();

                int id_max = (int)(check != null ? check.ID + 1 : 1);
                var checkAcc = _context.Account.Where(x => x.IsDeleted == false && x.UserName == dataRegister.UserName);
                var checkName = _context.Account.Where(x => x.IsDeleted == false && x.Name == dataRegister.Name);
                if (checkAcc != null && checkName != null)
                {
                    Account account = new Account
                    {
                        Code = "acc_" + id_max,
                        UserName = dataRegister.UserName,
                        Password = pass,
                        Name = dataRegister.Name,
                        Avatar = "cartoon.jpg",
                        CreatedBy = "admin",
                        CreatedTime = DateTime.Now,
                    };
                    _context.Account.Add(account);
                    _context.SaveChanges();
                    status = true;
                    title = "Đăng ký thành công.";
                }
                else
                {
                    status = false;
                    title = "Tài khoản đã tồn tại.";
                }
            }
            catch (Exception ex)
            {
                title = "Có lỗi xảy ra khi đăng ký tài khoản!";
            }
            var result = new
            {
                status = status,
                title = title
            };
            return Ok(result);
        }
    }
}
