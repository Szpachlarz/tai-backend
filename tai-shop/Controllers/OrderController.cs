using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using tai_shop.Dtos.Order;
using tai_shop.Enums;
using tai_shop.Interfaces;
using tai_shop.Mappers;
using tai_shop.Models;
using tai_shop.Services;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly CartManagementService _cartManagement;

        public OrderController(IOrderRepository orderRepository, ICartService cartService, CartManagementService cartManagement)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _cartManagement = cartManagement;
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var orderDtos = orders.Select(o => o.ToDto());
            return Ok(orderDtos);
        }

        //[Authorize(Policy = "AdminOrSelf")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order.ToDto());
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetCustomerOrders(string customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            var orderDtos = orders.Select(o => o.ToDto());
            return Ok(orderDtos);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder()
        {
            var cart = await _cartService.GetCartAsync();
            if (!cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty");

            await _cartManagement.TransitionToCheckout(cart.Id);

            var createdOrder = await _orderRepository.AddOrderAsync(cart);

            await _cartManagement.CompleteCart(cart.Id);

            var orderDto = createdOrder.ToDto();
            return CreatedAtAction(
                nameof(GetOrder),
                new { id = orderDto.Id },
                orderDto
            );
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDto>> DeleteOrder(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            if (existingOrder.UserId != userId)
            {
                return Forbid();
            }

            var deletedOrder = await _orderRepository.DeleteOrderAsync(id);
            if (deletedOrder == null)
            {
                return NotFound();
            }

            return Ok(deletedOrder.ToDto());
        }

        [HttpPatch("{id}/status")]
        //[Authorize(Policy = "AdminOnly")]
        [Authorize]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(int id, OrderStatus status)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            if (!IsValidStatusTransition(existingOrder.Status, status))
            {
                return BadRequest($"Invalid status transition from {existingOrder.Status} to {status}");
            }

            existingOrder.Status = status;
            var updatedOrder = await _orderRepository.UpdateOrderAsync(id, existingOrder);
            return Ok(updatedOrder.ToDto());
        }

        [HttpPatch("{id}/shipping-method")]
        //[Authorize(Policy = "AdminOrSelf")]
        public async Task<ActionResult<OrderDto>> UpdateShippingMethod(int id, ShippingMethod shippingMethod)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            if (existingOrder.Status >= OrderStatus.Shipped)
            {
                return BadRequest("Cannot change shipping method after order has been shipped");
            }

            existingOrder.ShippingMethod = shippingMethod;
            var updatedOrder = await _orderRepository.UpdateOrderAsync(id, existingOrder);
            return Ok(updatedOrder.ToDto());
        }        

        [HttpGet("by-status/{status}")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var filteredOrders = orders.Where(o => o.Status == status);
            return Ok(filteredOrders.Select(o => o.ToDto()));
        }

        private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                (OrderStatus.Pending, OrderStatus.Processing) => true,
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Shipped, OrderStatus.Delivered) => true,
                (_, OrderStatus.Cancelled) => true,
                _ => false
            };
        }
    }
}
