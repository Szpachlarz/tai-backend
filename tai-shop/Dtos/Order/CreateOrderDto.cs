using tai_shop.Enums;

namespace tai_shop.Dtos.Order
{
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
        public ShippingMethod ShippingMethod { get; set; }
    }
}
