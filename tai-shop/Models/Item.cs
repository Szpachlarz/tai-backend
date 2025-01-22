namespace tai_shop.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        //Orders
        public List<ItemOrder> ItemOrders { get; set; }
        //Return
        public List<ItemReturn> ItemReturns { get; set; }
        //Tags
        public List<ItemTag> ItemTags { get; set; }
        //Photos
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public List<Review> Reviews { get; set; }
        public double AverageRating => Reviews?.Any() == true
            ? Reviews.Average(r => r.Rating)
            : 0;
    }
}
