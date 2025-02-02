using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using tai_shop.Data;
using tai_shop.Enums;
using tai_shop.Exceptions;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetOrCreateCartId(HttpContext httpContext)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
                return userId;

            var anonymousId = httpContext.Request.Cookies["CartId"];
            if (anonymousId == null)
            {
                anonymousId = Guid.NewGuid().ToString();
                httpContext.Response.Cookies.Append("CartId", anonymousId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddDays(30)
                });
            }

            return $"anon_{anonymousId}";
        }

        public async Task<Cart> GetCartAsync()
        {
            var cartId = GetOrCreateCartId(_httpContextAccessor.HttpContext);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(c => c.UserId == cartId && c.Status == CartStatus.Active);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = cartId,
                    Status = CartStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddItemAsync(int itemId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));

            var cart = await GetCartAsync();
            var item = await _context.Items.FindAsync(itemId)
                ?? throw new NotFoundException("Item not found");

            if (item.StockQuantity < quantity)
                throw new InvalidOperationException("Not enough items in stock");

            var cartItem = cart.CartItems.FirstOrDefault(i => i.ItemId == itemId);
            if (cartItem != null)
            {
                if (item.StockQuantity < cartItem.Quantity + quantity)
                    throw new InvalidOperationException("Not enough items in stock");

                cartItem.Quantity += quantity;
                cartItem.UnitPrice = item.Price;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ItemId = itemId,
                    Quantity = quantity,
                    UnitPrice = item.Price,
                    CreatedAt = DateTime.UtcNow
                });
            }

            cart.LastUpdated = DateTime.UtcNow;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message;
                Console.WriteLine($"Database Error: {innerException}");
                throw;
            }
        }

        public async Task UpdateQuantityAsync(int itemId, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

            var cart = await GetCartAsync();
            var cartItem = cart.CartItems.FirstOrDefault(i => i.ItemId == itemId)
                ?? throw new NotFoundException("Item not found in cart");

            if (quantity == 0)
            {
                cart.CartItems.Remove(cartItem);
            }
            else
            {
                var item = await _context.Items.FindAsync(itemId)
                    ?? throw new NotFoundException("Item not found");

                if (item.StockQuantity < quantity)
                    throw new InvalidOperationException("Not enough items in stock");

                cartItem.Quantity = quantity;
                cartItem.UnitPrice = item.Price;
            }

            cart.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int itemId)
        {
            var cart = await GetCartAsync();
            var cartItem = cart.CartItems.FirstOrDefault(i => i.ItemId == itemId)
                ?? throw new NotFoundException("Item not found in cart");

            cart.CartItems.Remove(cartItem);
            cart.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync()
        {
            var cart = await GetCartAsync();
            cart.CartItems.Clear();
            cart.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetCartTotalAsync()
        {
            var cart = await GetCartAsync();
            return cart.CartItems.Sum(item => item.UnitPrice * item.Quantity);
        }
    }
}
