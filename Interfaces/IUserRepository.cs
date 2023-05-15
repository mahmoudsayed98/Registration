using WebApi_app.DTOs;
using WebApi_app.Models;

namespace WebApi_app.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);
        void UpdateUser(User user, ResetPasswordDto resetPasswordDto);
        User GetUserByUsername(string username);
        bool ValidateUserName(string username);
        bool VerifyPassword(User user, string password);
    }
}
 //hello from other side