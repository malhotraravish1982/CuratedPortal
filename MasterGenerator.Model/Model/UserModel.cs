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
        [Required]
        public string? Email { get; set; }
        [Required]
        [StringLength(maximumLength:8,ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!,@,#,etc) or (0-9),('A'-'Z'),('a'-'z')")]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? UserType { get; set; }
       
        [Phone]
        [DataType(DataType.PhoneNumber),MaxLength(10),MinLength(10,ErrorMessage ="Phone Number is must than 10 digits")]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; }
        public string? Address { get; set; }

    }
}
