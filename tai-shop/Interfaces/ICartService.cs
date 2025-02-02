using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync();
        Task AddItemAsync(int itemId, int quantity);
        Task UpdateQuantityAsync(int itemId, int quantity);
        Task RemoveItemAsync(int itemId);
        Task ClearCartAsync();
        Task<decimal> GetCartTotalAsync();
    }
}
