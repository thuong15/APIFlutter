using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project4.model;
using project4.ModelView;

namespace project4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly project4Context _context;

        public ValuesController(project4Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccount(string param1)
        {
            var accounts = from a in _context.Account.Where(x => x.IsDeleted == false)
                           select new
                           {
                               Name = a.Name + " " + param1
                           };

            if (accounts == null || !accounts.Any())
            {
                return NotFound();
            }

            return Ok(accounts);
        }

        [HttpPost("TestCallPost")]
        public async Task<IActionResult> TestCallPost([FromBody] ModelViewHome model)
        {
            var accounts = from a in _context.Account.Where(x => x.IsDeleted == false)
                           select new
                           {
                               Name = a.Name + " " + model.test
                           };

            if (accounts == null || !accounts.Any())
            {
                return NotFound();
            }

            return Ok(accounts);
        }
        [HttpPost("TestCallPost1")]
        public async Task<IActionResult> TestCallPost1([FromBody] ModelViewHome model)
        {
            var accounts = from a in _context.Account.Where(x => x.IsDeleted == false)
                           select new
                           {
                               Name = a.Name + " " + model.test
                           };

            if (accounts == null || !accounts.Any())
            {
                return NotFound();
            }

            return Ok(accounts);
        }

    }
}
