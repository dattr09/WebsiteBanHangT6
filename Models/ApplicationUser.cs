using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        public int? Age { get; set; } // Kiểu int? để có thể null
    }
}
