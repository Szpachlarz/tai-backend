using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tai_shop.ShopingCart;

namespace tai_shop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }  

        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}