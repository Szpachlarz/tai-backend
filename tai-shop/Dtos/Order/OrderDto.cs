using tai_shop.Enums;

namespace tai_shop.Dtos.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
