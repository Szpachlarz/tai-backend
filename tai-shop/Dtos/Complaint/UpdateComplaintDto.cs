using tai_shop.Enums;

namespace tai_shop.Dtos.Complaint
{
    public class UpdateComplaintDto
    {
        public string Description { get; set; }
        public ComplaintStatus? Status { get; set; }
    }
}
