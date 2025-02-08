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
        //Returns
        public List<Return> Returns { get; set; }
        public Payment? Payment { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public DateTime? PaymentDate { get; set; }
        public decimal TotalAmount =>
        ItemOrders?.Sum(io => io.Quantity * io.Price) ?? 0;
        public decimal FinalAmount => TotalAmount + GetShippingCost();
        private decimal GetShippingCost()
        {
            return ShippingMethod switch
            {
                ShippingMethod.InPost => 5.99m,
                ShippingMethod.DHL => 15.99m,
                ShippingMethod.DPD => 25.99m,
                _ => 0m
            };
        }
    }
}
