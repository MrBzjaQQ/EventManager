using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EventManager.Models
{
    public class UserModel: IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
    }
}