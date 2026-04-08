namespace WebApp.Models
{
    public class UserDetailVM
    {
        public int Udid { get; set; }

        public int UserId { get; set; }

        public IFormFile? Photo { get; set; }
     
        public string? Address { get; set; }

        public string? City { get; set; }

    }
}
