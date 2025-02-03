using tai_shop.Enums;

namespace tai_shop.Dtos.Order
{
    public class CreateOrderDto
    {
        public AddressDto Address { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
    }
}
