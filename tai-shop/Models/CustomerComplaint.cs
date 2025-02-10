using tai_shop.Enums;

namespace tai_shop.Models
{
    public class CustomerComplaint
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public string Description { get; set; }
        public ComplaintStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get ; set; }
        public DateTime? ResolvedDate { get; set; }
    }
}
