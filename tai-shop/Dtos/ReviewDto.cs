using System.ComponentModel.DataAnnotations;

namespace tai_shop.Dtos
{
    public class ReviewDto
    {
        public int ItemId { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        [StringLength(1000)]
        public string Comment { get; set; }
    }
}
