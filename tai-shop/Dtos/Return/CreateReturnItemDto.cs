namespace tai_shop.Dtos.Return
{
    public class CreateReturnItemDto
    {
        public int ItemOrderId { get; set; }
        public int QuantityToReturn { get; set; }
    }
}
