using System.ComponentModel.DataAnnotations;

namespace WebAppRazor.Pages.Models
{
    public class UserProfileInput
    {
        [Required]
        public int UserId { get; set; }

        public int Udid { get; set; } = 0;

        public byte[]? Photo { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? PostCode { get; set; }

        public string? Phone { get; set; }
    }
}
