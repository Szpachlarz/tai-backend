namespace tai_shop.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
