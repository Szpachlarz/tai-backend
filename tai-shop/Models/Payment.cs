using tai_shop.Enums;

namespace tai_shop.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? FailureReason { get; set; }
        public bool IsRefunded { get; set; }
        public DateTime? RefundDate { get; set; }
        public string? RefundTransactionId { get; set; }
        public decimal? RefundAmount { get; set; }
    }
}
