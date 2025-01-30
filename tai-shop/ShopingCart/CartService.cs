namespace tai_shop.ShopingCart
{
    public class CartService
    {
        private readonly List<CartItem> _items = new List<CartItem>();

        public void AddProduct(Produkt product, int quantity = 1)
        {
            var existingItem = _items.FirstOrDefault(item => item.Produkt.Id == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem { Produkt = product, Quantity = quantity });
            }
        }
        public void RemoveProduct(int productId)
        {
            var item = _items.FirstOrDefault(i => i.Produkt.Id == productId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }
        public void UpdateQuantity(int productId, int newQuantity)
        {
            var item = _items.FirstOrDefault(i => i.Produkt.Id == productId);
            if (item != null)
            {
                if (newQuantity <= 0)
                    _items.Remove(item);
                else
                    item.Quantity = newQuantity;
            }
        }
        public void DisplayCart()
        {
            Console.WriteLine("Koszyk:");
            foreach (var item in _items)
            {
                Console.WriteLine($"Produkt: {item.Produkt.Name}, Ilość: {item.Quantity}, Cena: {item.Produkt.Price:C}, Łącznie: {item.TotalPrice:C}");
            }
            Console.WriteLine($"Łączna wartość koszyka: {_items.Sum(item => item.TotalPrice):C}");
        }  
        public void ClearCart()
        {
            _items.Clear();
        }
    }
}
