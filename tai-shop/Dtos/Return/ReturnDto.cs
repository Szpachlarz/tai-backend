using tai_shop.Dtos.Order;
using tai_shop.Enums;

namespace tai_shop.Dtos.Return
{
    public class ReturnDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime ReturnRequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public ReturnStatus Status { get; set; }
        public string Reason { get; set; }
        public string? CustomerNotes { get; set; }
        public string? AdminNotes { get; set; }
        public decimal RefundAmount { get; set; }
        public OrderDto Order { get; set; }
        public List<ReturnItemDto> ReturnItems { get; set; }
    }
}
