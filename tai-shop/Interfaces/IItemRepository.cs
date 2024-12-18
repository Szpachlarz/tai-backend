using tai_shop.Dtos.Item;
using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(int id);
        Task<Item> CreateAsync(Item item);
        Task<Item?> UpdateAsync(int id, UpdateItemRequestDto itemDto);
        Task<Item?> DeleteAsync(int id);
    }
}
