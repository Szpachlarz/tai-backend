using tai_shop.Enums;

namespace tai_shop.Dtos.Cart
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public CartStatus Status { get; set; }
    }
}
