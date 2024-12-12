using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace tai_shop.Models
{
    public class ItemReturn
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        [ForeignKey("Return")]
        public int ReturnId { get; set; }
        public Return Return { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }        
    }
}
