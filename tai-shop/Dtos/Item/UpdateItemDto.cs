using System.ComponentModel.DataAnnotations;

namespace tai_shop.Dtos.Item
{
    public class UpdateItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<IFormFile>? Photos { get; set; }
        public List<int>? PhotosToDelete { get; set; }
    }
}
