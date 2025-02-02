using System.ComponentModel.DataAnnotations;

namespace tai_shop.Dtos.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
