using Microsoft.EntityFrameworkCore;
using tai_shop.Data;
using tai_shop.Interfaces;
using tai_shop.Models;

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
            return await _context.Items.ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<Item?> UpdateAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
