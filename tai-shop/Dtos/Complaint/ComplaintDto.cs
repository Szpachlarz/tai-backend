using tai_shop.Enums;

namespace tai_shop.Dtos.Complaint
{
    public class ComplaintDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Description { get; set; }
        public ComplaintStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
    }
}
