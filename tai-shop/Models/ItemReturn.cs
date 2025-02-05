using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace tai_shop.Models
{
    public class ItemReturn
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ReturnId { get; set; }
        public Return Return { get; set; }
        public int ItemOrderId { get; set; }
        public ItemOrder ItemOrder { get; set; }
    }
}
