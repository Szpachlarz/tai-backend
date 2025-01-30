using System.ComponentModel.DataAnnotations.Schema;

namespace tai_shop.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        [ForeignKey("Item")]
        public string ItemId { get; set; }
        public Item Item { get; set; }
    }
}
