using Microsoft.EntityFrameworkCore;
using tai_shop.Data;
using tai_shop.Dtos.Payment;
using tai_shop.Enums;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Payment> ProcessPayment(PaymentDto paymentDto)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == paymentDto.OrderId);

            if (order == null)
                throw new ArgumentException("Order not found");

            var payment = new Payment
            {
                OrderId = paymentDto.OrderId,
                Amount = paymentDto.Amount,
                PaymentMethod = paymentDto.PaymentMethod,
                PaymentDate = DateTime.UtcNow,
                Status = PaymentStatus.Pending
            };

            try
            {
                switch (paymentDto.PaymentMethod)
                {
                    case PaymentMethod.CreditCard:
                        await ProcessCreditCardPayment(paymentDto, payment);
                        break;
                    case PaymentMethod.BankTransfer:
                        await ProcessBankTransfer(paymentDto, payment);
                        break;
                    default:
                        throw new ArgumentException("Invalid payment method");
                }

                payment.Status = PaymentStatus.Successful;
            }
            catch (Exception ex)
            {
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = ex.Message;
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        private async Task ProcessCreditCardPayment(PaymentDto paymentDto, Payment payment)
        {
            if (string.IsNullOrEmpty(paymentDto.CardNumber) ||
                string.IsNullOrEmpty(paymentDto.ExpiryDate) ||
                string.IsNullOrEmpty(paymentDto.CVV))
            {
                throw new ArgumentException("Invalid card details");
            }

            await Task.Delay(1000);
            payment.TransactionId = Guid.NewGuid().ToString();
        }

        private async Task ProcessBankTransfer(PaymentDto paymentDto, Payment payment)
        {
            if (string.IsNullOrEmpty(paymentDto.BankAccountNumber) ||
                string.IsNullOrEmpty(paymentDto.BankName) ||
                string.IsNullOrEmpty(paymentDto.IBAN) ||
                string.IsNullOrEmpty(paymentDto.SWIFTCode) ||
                string.IsNullOrEmpty(paymentDto.AccountHolderName))
            {
                throw new ArgumentException("Invalid card details");
            }

            await Task.Delay(1000);
            payment.TransactionId = Guid.NewGuid().ToString();
        }

        public async Task<Payment> RefundPayment(int orderId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);

            if (payment == null)
                throw new ArgumentException("Payment not found");

            if (payment.Status != PaymentStatus.Successful)
                throw new InvalidOperationException("Cannot refund non-successful payment");

            try
            {
                switch (payment.PaymentMethod)
                {
                    case PaymentMethod.CreditCard:
                        await ProcessCreditCardRefund(payment);
                        break;
                    case PaymentMethod.BankTransfer:
                        await ProcessBankTransferRefund(payment);
                        break;
                }

                payment.Status = PaymentStatus.Refunded;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Refund failed: {ex.Message}");
            }

            return payment;
        }

        private async Task ProcessCreditCardRefund(Payment payment)
        {
            // Implement credit card refund logic
            await Task.Delay(1000);
        }

        private async Task ProcessBankTransferRefund(Payment payment)
        {
            // Implement bank transfer refund logic
            await Task.Delay(1000);
        }

        public async Task<Payment> GetPaymentByOrderId(int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<List<Payment>> GetUserPayments(string userId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Where(p => p.Order.UserId == userId)
                .ToListAsync();
        }
    }
}
