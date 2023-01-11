using EmployeeManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using EmployeeManagementAPI.Helper;
using System.Text;
using System.Text.RegularExpressions;

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
                if (await CheckingExistingEmail(register.Email))
                {
                    return BadRequest(new { Message = "EmaiId Already Registered" });
                }
               
                if(await CheckingUserName(register.UserName))
                {
                    return BadRequest(new { message = "Username already exist!" });
                }
                
                var pass = CheckPasswordStrength(register.Password);
                if(!string.IsNullOrEmpty(pass))
                {
                    return BadRequest(new { message = pass.ToString() });
                }

                if (register == null)
                {
                    return BadRequest();
                }
                else
                {
                    register.Password = Helper.HashingPassword.HashPassword(register.Password);
                    register.Role = "user";
                    register.Token = "";
                    await dBContext.Registers.AddAsync(register);
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

        private async Task<bool> CheckingExistingEmail(string emailId)
        {
            return await dBContext.Registers.AnyAsync(x => x.Email.Equals(emailId));
        }

        private async Task<bool> CheckingUserName(string userName)
        {
            return await dBContext.Registers.AnyAsync(x => x.UserName.Equals(userName));
        }

        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if(password.Length < 8)
            {
                sb.Append("Password length should be minimum 8 characters" + Environment.NewLine);
            }
            if(!(Regex.IsMatch(password,"[a-z]") && !Regex.IsMatch(password,"[A-Z]") && !Regex.IsMatch(password,"[0-9]")))
            {
                sb.Append("Password should be alphanumeric" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(password, "[<,>,@,#,$,%,*,^,:,;,{,},[,],+,=]")))
            {
                sb.Append("Password should contain special characters" + Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
