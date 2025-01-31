namespace tai_shop.Dtos.Order
{
    public class OrderItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
