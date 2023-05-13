using Microsoft.AspNetCore.Mvc;
using WebApi_app.Interfaces;
using WebApi_app.Models;
using WebApi_app.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using WebApi_app.Repositories;
using WebApi_app.Services;

namespace WebApi_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly EncryptionService EncryptionService = new EncryptionService();
        private int userId;

        public RegistrationController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("Register")]
        public IActionResult RegisterUser([FromBody] UserRegistrationDto userRegistrationDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the username already exists
            if (!userRepository.ValidateUserName(userRegistrationDto.UserName))
            {
                return Conflict("Username already exists");
            }


            // Create a new User object
            User user = new User
            {
                FirstName = userRegistrationDto.FirstName,
                FatherName = userRegistrationDto.FatherName,
                FamilyName = userRegistrationDto.FamilyName,
                Address = userRegistrationDto.Address,
                Password = userRegistrationDto.Password,
                UserName = userRegistrationDto.UserName,
                BirthDate = userRegistrationDto.BirthDate.ToString()
            };
            userRepository.AddUser(user);
            return Ok("User registered successfully");
        }
        [HttpPost("/api/user/profile")]
        public IActionResult GetUserProfileData([FromBody] UserCredentialsDto credentials)
        {
            string username = credentials.Username;
            string password = EncryptionService.HashPassword(credentials.Password);
            User user = userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return NotFound(); 
            }
            
            if(user.Password != password)
            {
                return NotFound();
            }
            user.Address = EncryptionService.DecryptPersonalData(user.Address, credentials.Password);
            user.BirthDate = EncryptionService.DecryptPersonalData(user.BirthDate, credentials.Password);
            return Ok(user);
        }
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            string username = resetPasswordDto.Username;
            string oldpassword = resetPasswordDto.OldPassword;
            string newpassword = EncryptionService.HashPassword(resetPasswordDto.NewPassword);
            User user = userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return NotFound();
            }
            if (!userRepository.VerifyPassword(user, oldpassword))
            {
                return BadRequest("Incorrect old password");
            }
            user.Password = newpassword;
            userRepository.UpdateUser(user, resetPasswordDto);
            user.Address = EncryptionService.DecryptPersonalData(user.Address, resetPasswordDto.NewPassword);
            user.BirthDate = EncryptionService.DecryptPersonalData(user.BirthDate, resetPasswordDto.NewPassword);
            return Ok(user);
            }


    }
}