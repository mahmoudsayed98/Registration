using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace WebApi_app.DTOs
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "FatherName is required.")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "FamilyName is required.")]
        public string FamilyName { get; set; }
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", ErrorMessage = "Password must have at least one lowercase letter, one uppercase letter, and one digit.")]
        public string Password { get; set; }

        }
}
