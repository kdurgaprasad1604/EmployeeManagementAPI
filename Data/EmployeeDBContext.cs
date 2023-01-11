using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Data
{
    public class EmployeeDBContext:DbContext
    {
        public EmployeeDBContext(DbContextOptions options) : base(options)  
        {

        }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<Login> Logins { get; set; }
        public DbSet<Register> Registers { get; set; }
    }
}
