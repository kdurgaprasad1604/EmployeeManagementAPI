using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models
{
    public class Login
    {
        [Key]
        public int LoginId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
