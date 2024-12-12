using System.ComponentModel.DataAnnotations.Schema;

namespace tai_shop.Models
{
    public class ItemTag
    {
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [ForeignKey("Tag")]
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
