using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using WebApplication5.Models;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDBContext context;
        public EmployeeController(EmployeeDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            return Ok(await context.Employees.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployee employee)
        {

            if (employee != null)
            {
                var emp = new Employee()
                {
                    Id = Guid.NewGuid(),
                    Name = employee.Name,
                    Phone = employee.Phone,
                    Email = employee.Email,
                    Salary = employee.Salary
                };
                await context.Employees.AddAsync(emp);
                await context.SaveChangesAsync();
                return Ok(emp);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, UpdateEmployee employee)
        {
            var employees = await context.Employees.FindAsync(id);
            if (employees != null)
            {
                employees.Name = employee.Name;
                employees.Phone = employee.Phone;
                employees.Email = employee.Email;
                employees.Salary = employee.Salary;
            }
            await context.SaveChangesAsync();
            return Ok(employees);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee([FromRoute] Guid id)
        {
            var empId = context.Employees.Find(id);
            if (empId != null)
            {
                context.Employees.Remove(empId);
                context.SaveChanges();
                return Ok(empId);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var emp = await context.Employees.FindAsync(id);
            if (emp == null)
            {
                return NotFound();

            }
            else
            {
                return Ok(emp);
            }
        }
    }
}
