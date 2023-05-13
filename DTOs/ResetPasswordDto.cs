using System.ComponentModel.DataAnnotations;

namespace WebApi_app.DTOs
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
