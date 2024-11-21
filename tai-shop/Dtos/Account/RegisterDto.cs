using System.ComponentModel.DataAnnotations;

namespace tai_shop.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password
        {
            get; set;
        }
    }
}
