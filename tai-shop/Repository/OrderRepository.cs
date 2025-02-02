using Microsoft.EntityFrameworkCore;
using System;
using tai_shop.Data;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.ItemOrders)
                .ThenInclude(io => io.Item)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.ItemOrders)
                .ThenInclude(io => io.Item)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await _context.Orders
                .Include(o => o.ItemOrders)
                .ThenInclude(io => io.Item)
                .Where(o => o.UserId == customerId)
                .ToListAsync();
        }

        public async Task<Order> AddOrderAsync(Cart cart)
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                ItemOrders = cart.CartItems.Select(ci => new ItemOrder
                {
                    ItemId = ci.ItemId,
                    Quantity = ci.Quantity,
                    Price = ci.UnitPrice
                }).ToList()
            };

            if (cart.UserId?.StartsWith("anon_") != true)
            {
                order.UserId = cart.UserId;
            }
            else
            {
                order.AnonymousUserId = cart.UserId;
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateOrderAsync(int orderId, Order order)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            if (existingOrder == null)
            {
                return null;
            }

            existingOrder.ItemOrders = order.ItemOrders;
            await _context.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<Order?> DeleteOrderAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
