using System.ComponentModel.DataAnnotations.Schema;

namespace tai_shop.Models
{
    public class Return
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        //Items
        public List<ItemReturn> ItemReturns { get; set; }
    }
}
