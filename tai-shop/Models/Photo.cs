namespace tai_shop.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public string Filepath { get; set; }
        public string Filename { get; set; }
        public long Length { get; set; }
    }
}
