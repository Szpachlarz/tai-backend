namespace tai_shop.Dtos
{
    public class ItemFilter
    {
        public string? SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinStock { get; set; }
        public double? MinRating { get; set; }
        public List<string>? Tags { get; set; }
        public bool? HasDiscount { get; set; }
        //public string? SortBy { get; set; }
        //public bool SortDescending { get; set; }
    }
}
