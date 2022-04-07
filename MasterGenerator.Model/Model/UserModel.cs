using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Model.Model
{
    public class UserModel
    {
        public int Id { get; set; }       
        [Required]
        public string? FirstName { get; set; }      
        public string? LastName { get; set; }
        public string? Email { get; set; }
        [Required]
        [StringLength(maximumLength:8,ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!,@,#,etc) or (0-9),('A'-'Z'),('a'-'z')")]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
        public string? UserType { get; set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber),MaxLength(10),]
        public string? PhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "User name contain at least one symbol (!,@,#,etc) or (0-9),(A-Z),(a-z)")]
        public string Username { get; set; }
        public string? Address { get; set; }

    }
}
