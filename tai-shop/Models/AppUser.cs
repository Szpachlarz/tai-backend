using Microsoft.AspNetCore.Identity;

namespace tai_shop.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
