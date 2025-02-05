namespace tai_shop.Dtos.Return
{
    public class CreateReturnDto
    {
        public int OrderId { get; set; }
        public List<ReturnItemDto> ReturnItems { get; set; }
        public string Reason { get; set; }
        public string CustomerNotes { get; set; }
    }
}
