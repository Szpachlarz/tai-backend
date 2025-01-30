using System.ComponentModel.DataAnnotations;

namespace tai_shop.Dtos.Item
{
    public class CreateItemRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
