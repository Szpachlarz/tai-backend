using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using tai_shop.Dtos.Cart;
using tai_shop.Enums;
using tai_shop.Interfaces;
using tai_shop.Mappers;
using tai_shop.Models;
using tai_shop.Services;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            var cart = await _cartService.GetCartAsync();
            return Ok(cart.ToCartDto());
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem(AddCartItemDto addCartItemDto)
        {
            await _cartService.AddItemAsync(addCartItemDto.ItemId, addCartItemDto.Quantity);
            return Ok();
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateQuantity(int itemId, UpdateCartItemDto updateCartItemDto)
        {
            await _cartService.UpdateQuantityAsync(itemId, updateCartItemDto.Quantity);
            return Ok();
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            await _cartService.RemoveItemAsync(itemId);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync();
            return Ok();
        }

        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetTotal()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var total = await _cartService.GetCartTotalAsync();
            return Ok(total);
        }
    }
}
