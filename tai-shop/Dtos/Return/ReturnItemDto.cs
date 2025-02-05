using tai_shop.Dtos.Item;

namespace tai_shop.Dtos.Return
{
    public class ReturnItemDto
    {
        public int Id { get; set; }
        public int ItemOrderId { get; set; }
        public int QuantityToReturn { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public string? ProductImageUrl { get; set; }
        public decimal SubTotal => Price * QuantityToReturn;
        public ItemDto Product { get; set; }
    }
}
