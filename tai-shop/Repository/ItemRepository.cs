using Microsoft.EntityFrameworkCore;
using tai_shop.Data;
using tai_shop.Dtos.Item;
using tai_shop.Interfaces;
using tai_shop.Models;
using tai_shop.Services;

namespace tai_shop.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Item> CreateAsync(Item item)
        {
            await _context.Items.AddAsync(item);

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item?> DeleteAsync(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return null;
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _context.Items.Include(i => i.Photos).ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items.Include(i => i.Photos).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Item>> GetItemsByIdsAsync(IEnumerable<int> itemIds)
        {
            return await _context.Items
                .Where(i => itemIds.Contains(i.Id))
                .ToListAsync();
        }

        public async Task<Item?> UpdateAsync(int id, UpdateItemDto itemDto)
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (existingItem == null)
            {
                return null;
            }

            existingItem.Name = itemDto.Name;
            existingItem.Description = itemDto.Description;
            existingItem.Price = itemDto.Price;

            await _context.SaveChangesAsync();

            return existingItem;
        }

        public async Task<Item?> UpdateStockQuantityAsync(int id, UpdateStockQuantityDto itemDto)
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (existingItem == null)
            {
                return null;
            }

            existingItem.StockQuantity = itemDto.StockQuantity;

            await _context.SaveChangesAsync();

            return existingItem;
        }

        public async Task<bool> ItemExistsAsync(int itemId)
        {
            return await _context.Items.AnyAsync(i => i.Id == itemId);
        }
    }
}
