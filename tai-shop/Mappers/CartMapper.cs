using tai_shop.Dtos.Cart;
using tai_shop.Models;

namespace tai_shop.Mappers
{
    public static class CartMapper
    {
        public static CartDto ToCartDto(this Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    Id = item.Id,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    CreatedAt = item.CreatedAt
                }).ToList(),
                CreatedAt = cart.CreatedAt,
                LastUpdated = cart.LastUpdated,
                Status = cart.Status
            };
        }
    }
}
