using Microsoft.EntityFrameworkCore;
using tai_shop.Models;
using tai_shop.ShopingCart;

namespace tai_shop.Data
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
        {
        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartService> Carts { get; set; }
        public DbSet<Produkt> Produkty { get; set; }
    }
}