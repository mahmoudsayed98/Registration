using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApi_app.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string FamilyName { get; set; }
        public string BirthDate { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
