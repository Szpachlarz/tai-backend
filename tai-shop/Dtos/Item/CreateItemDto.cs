using System.ComponentModel.DataAnnotations;

namespace tai_shop.Dtos.Item
{
    public class CreateItemDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<int> TagIds { get; set; }
    }
}
