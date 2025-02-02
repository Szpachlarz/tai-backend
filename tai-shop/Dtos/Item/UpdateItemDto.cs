using System.ComponentModel.DataAnnotations;

namespace tai_shop.Dtos.Item
{
    public class UpdateItemDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public IEnumerable<IFormFile> Photos { get; set; }
        public List<int> PhotosToDelete { get; set; }
    }
}
