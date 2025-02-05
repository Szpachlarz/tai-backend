using System.ComponentModel.DataAnnotations.Schema;
using tai_shop.Enums;

namespace tai_shop.Models
{
    public class Return
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public DateTime ReturnRequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public ReturnStatus Status { get; set; }
        public string Reason { get; set; }
        public string CustomerNotes { get; set; }
        public decimal RefundAmount { get; set; }
        //Items
        public List<ItemReturn> ItemReturns { get; set; }
    }
}
