using System.ComponentModel.DataAnnotations.Schema;
using tai_shop.Enums;

namespace tai_shop.Models
{
    public class Order
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public string? AnonymousUserId { get; set; }
        public int ShippingAddressId { get; set; }
        public Address ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        //Items
        public List<ItemOrder> ItemOrders { get; set; }
        public decimal TotalAmount =>
        ItemOrders?.Sum(io => io.Quantity * io.Price) ?? 0;
    }
}
