using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tai_shop.ShopingCart;
using tai_shop.Data;  
using tai_shop.Models;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ShoppingCartContext _context;

        public CartController(ShoppingCartContext context)
        {
            _context = context;
        }

        // Pobierz zawartość koszyka
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Produkt)
                .ToListAsync();

            return Ok(cartItems.Select(item => new
            {
                item.Id,
                Product = item.Produkt.Name,
                item.Produkt.Price,
                item.Quantity,
                TotalPrice = item.Produkt.Price * item.Quantity
            }));
        }

        // Dodaj produkt do koszyka
        [HttpPost]
        public async Task<IActionResult> AddToCart(Produkt productId, int quantity = 1)
        {
            var product = await _context.Produkty.FindAsync(productId);
            if (product == null) return NotFound("Produkt nie istnieje.");

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.Produkt == productId); ///Id
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem { Produkt = productId, Quantity = quantity };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok("Produkt dodany do koszyka.");
        }

        // Aktualizuj ilość
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null) return NotFound("Element koszyka nie istnieje.");

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return Ok("Ilość zaktualizowana.");
        }

        // Usuń produkt z koszyka
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null) return NotFound("Element koszyka nie istnieje.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return Ok("Produkt usunięty z koszyka.");
        }
    }
}
