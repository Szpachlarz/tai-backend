using System.ComponentModel.DataAnnotations.Schema;

namespace tai_shop.Models
{
    public class ItemOrder
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
