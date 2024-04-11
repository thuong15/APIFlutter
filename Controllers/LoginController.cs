using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

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
            bool login = false;
            var checkUser = _context.Account.FirstOrDefault(x=>x.IsDeleted == false 
                                                && x.UserName == modelViewLogin.UserName 
                                                && x.Password == modelViewLogin.Password);
            if (checkUser != null)
            {
                login = true;
            }
            else
            {
                login = false;
            }
            return Ok(login);
        }
    }
}
