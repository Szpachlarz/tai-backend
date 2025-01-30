using tai_shop.Dtos.Account;
using tai_shop.Models;

namespace tai_shop.Dtos.Item
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public List<PhotoDto> Photos { get; set; }
        public List<ItemTag> ItemTags { get; set; }
    }
}
