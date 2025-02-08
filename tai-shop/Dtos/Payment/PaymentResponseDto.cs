using tai_shop.Enums;

namespace tai_shop.Dtos.Payment
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string? FailureReason { get; set; }
    }
}
