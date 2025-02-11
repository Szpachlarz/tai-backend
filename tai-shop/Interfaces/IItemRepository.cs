using tai_shop.Dtos;
using tai_shop.Dtos.Item;
using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(int id);
        Task<IEnumerable<Item>> GetItemsByIdsAsync(IEnumerable<int> itemIds);
        Task<Item> CreateAsync(Item item, List<int> tagIds);
        Task<Item?> UpdateAsync(int id, UpdateItemDto itemDto);
        Task<Item?> DeleteAsync(int id);
        Task<Item?> UpdateStockQuantityAsync(int id, UpdateStockQuantityDto itemDto);
        Task<bool> ItemExistsAsync(int itemId);
        Task<IEnumerable<Item>> GetFilteredItemsAsync(ItemFilter filter);
    }
}
