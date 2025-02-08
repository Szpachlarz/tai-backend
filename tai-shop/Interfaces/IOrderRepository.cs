using tai_shop.Dtos;
using tai_shop.Dtos.Payment;
using tai_shop.Enums;
using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);
        Task<Order> AddOrderAsync(Cart cart, AddressDto addressDto, ShippingMethod shippingMethod);
        Task<Order?> UpdateOrderAsync(int orderId, Order order);
        Task<Order?> DeleteOrderAsync(int orderId);
        Task<PaymentResponseDto> ProcessOrderPaymentAsync(int orderId, CreatePaymentDto paymentDto);
        Task<Payment> RefundOrderAsync(int orderId);
    }
}
