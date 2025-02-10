using System.ComponentModel.DataAnnotations;

namespace tai_shop.Dtos.Complaint
{
    public class CreateComplaintDto
    {
        public int OrderId { get; set; }
        public string Description { get; set; }
    }
}
