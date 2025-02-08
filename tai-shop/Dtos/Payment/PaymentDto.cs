using tai_shop.Enums;

namespace tai_shop.Dtos.Payment
{
    public class PaymentDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        //card
        public string? CardNumber { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }
        //bank transfer
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? IBAN { get; set; }
        public string? SWIFTCode { get; set; }
        public string? AccountHolderName { get; set; }
    }
}
