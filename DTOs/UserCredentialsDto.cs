using System.ComponentModel.DataAnnotations;

namespace WebApi_app.DTOs
{
    public class UserCredentialsDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
