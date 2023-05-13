using Microsoft.EntityFrameworkCore;
using WebApi_app.DTOs;
using WebApi_app.Interfaces;
using WebApi_app.Models;
using WebApi_app.Services;

namespace WebApi_app.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly EncryptionService EncryptionService = new EncryptionService();
        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public User GetUserByUsername(string username)
        {

            return appDbContext.Users.FirstOrDefault(u => u.UserName == username);
        }
        public void AddUser(User user)
        {
            user.Address = EncryptionService.EncryptPersonalData(user.Address, user.Password);
            user.BirthDate = EncryptionService.EncryptPersonalData(user.BirthDate.ToString(), user.Password);
            user.Password = EncryptionService.HashPassword(user.Password);

            appDbContext.Add(user);
            appDbContext.SaveChanges();
        }
        public void UpdateUser(User user, ResetPasswordDto resetPasswordDto)
        {
            user.Address = EncryptionService.DecryptPersonalData(user.Address, resetPasswordDto.OldPassword);
            user.BirthDate = EncryptionService.DecryptPersonalData(user.BirthDate, resetPasswordDto.OldPassword);
            user.Address = EncryptionService.EncryptPersonalData(user.Address, resetPasswordDto.NewPassword);
            user.BirthDate = EncryptionService.EncryptPersonalData(user.BirthDate, resetPasswordDto.NewPassword);

            appDbContext.Users.Update(user);
            appDbContext.SaveChanges();
        }
        public bool ValidateUserName(string username)
        {
            return appDbContext.Users.Where(u => u.UserName == username).Count()==0;
        }
        public bool VerifyPassword(User user, string password)
        {
            string hashedPassword = EncryptionService.HashPassword(password);
             return user.Password == hashedPassword;
        }

        public User GetUserById(int userId)
        {
            var userEntity = appDbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (userEntity == null)
            {
                return null; 
            }

            var user = new User
            {
                FirstName = userEntity.FirstName,
                FatherName = userEntity.FatherName,
                FamilyName = userEntity.FamilyName,
            };

            return user;
        }
    }
}
