namespace tai_shop.ShopingCart
{
    public class CartItem
    {
        public int Id { get; set; }
        public Produkt Produkt { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice => Produkt.Price * Quantity;
    }
}
