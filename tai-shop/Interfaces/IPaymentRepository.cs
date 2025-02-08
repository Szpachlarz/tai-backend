using tai_shop.Dtos.Payment;
using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> ProcessPayment(PaymentDto paymentDto);
        Task<Payment> RefundPayment(int orderId);
        Task<Payment> GetPaymentByOrderId(int orderId);
        Task<List<Payment>> GetUserPayments(string userId);
    }
}
