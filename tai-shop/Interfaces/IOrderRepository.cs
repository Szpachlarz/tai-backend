using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);
        Task<Order> AddOrderAsync(Cart cart);
        Task<Order?> UpdateOrderAsync(int orderId, Order order);
        Task<Order?> DeleteOrderAsync(int orderId);
    }
}
