using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using tai_shop.Data;
using tai_shop.Enums;
using tai_shop.Exceptions;
using tai_shop.Models;

namespace tai_shop.Services
{
    public class CartManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly TimeSpan _cleanupThreshold = TimeSpan.FromDays(30);

        public CartManagementService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> TransitionToCheckout(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == cartId)
                ?? throw new NotFoundException("Cart not found");

            if (cart.Status != CartStatus.Active)
            {
                throw new InvalidOperationException("Only active carts can be checked out");
            }

            await ValidateInventory(cart);
            await LockPrices(cart);

            cart.Status = CartStatus.CheckingOut;
            cart.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> CompleteCart(int cartId)
        {
            var cart = await _context.Carts
                .FindAsync(cartId)
                ?? throw new NotFoundException("Cart not found");

            if (cart.Status != CartStatus.CheckingOut)
            {
                throw new InvalidOperationException("Only carts in checkout can be completed");
            }

            cart.Status = CartStatus.Converted;
            cart.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return cart;
        }

        private async Task ValidateInventory(Cart cart)
        {
            foreach (var item in cart.CartItems)
            {
                var product = await _context.Items.FindAsync(item.Id);
                if (product == null || product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Insufficient inventory for product {item.Id}");
                }
            }
        }

        private async Task LockPrices(Cart cart)
        {
            foreach (var item in cart.CartItems)
            {
                var product = await _context.Items.FindAsync(item.ItemId);
                item.UnitPrice = product.Price;
            }
        }
    }
}
