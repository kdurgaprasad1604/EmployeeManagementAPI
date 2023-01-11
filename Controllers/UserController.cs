using EmployeeManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private EmployeeDBContext dBContext = null;
        public UserController(EmployeeDBContext employeeDB)
        {
            this.dBContext = employeeDB;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] Register register)
        {
            if(register == null)
            {
                return BadRequest();
            }

            var user = await dBContext.Registers.FirstOrDefaultAsync(x => x.UserName == register.UserName && x.Password == register.Password);
            if(user == null)
            {
                return NotFound(new {message = "Details Not Found"});
            }
            return Ok(new { message = "Login Success" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            try
            {
                if (register == null)
                {
                    return BadRequest();
                }
                else
                {
                    var reg = await dBContext.Registers.AddAsync(register);
                    await dBContext.SaveChangesAsync();
                    return Ok(new { message = "Registered Sucessfully" });
                }
                
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return Ok();
        }
    }
}
