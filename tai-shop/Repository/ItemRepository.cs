using Microsoft.EntityFrameworkCore;
using System.Linq;
using tai_shop.Data;
using tai_shop.Dtos;
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

        public async Task<Item> CreateAsync(Item item, List<int> tagIds)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            if (tagIds != null && tagIds.Any())
            {
                var existingTagIds = await _context.ItemTags
            .Where(it => it.ItemId == item.Id)
            .Select(it => it.TagId)
            .ToListAsync();

                var newTagIds = tagIds.Where(tagId => !existingTagIds.Contains(tagId)).ToList();

                if (newTagIds.Any())
                {
                    var itemTags = newTagIds.Select(tagId => new ItemTag
                    {
                        ItemId = item.Id,
                        TagId = tagId
                    }).ToList();

                    _context.ItemTags.AddRange(itemTags);
                    await _context.SaveChangesAsync();
                }
            }

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
            return await _context.Items
                .Include(i => i.Photos)
                .Include(i => i.Reviews)
                .Include(i => i.ItemTags)
                    .ThenInclude(it => it.Tag)
                .ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items
                .Include(i => i.Photos)
                .Include(i => i.Reviews)
                .Include(i => i.ItemTags)
                    .ThenInclude(it => it.Tag)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Item>> GetItemsByIdsAsync(IEnumerable<int> itemIds)
        {
            return await _context.Items
                .Where(i => itemIds.Contains(i.Id))
                .ToListAsync();
        }

        public async Task<Item?> UpdateAsync(int id, UpdateItemDto itemDto)
        {
            var existingItem = await _context.Items
                .Include(i => i.ItemTags)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingItem == null)
            {
                return null;
            }

            existingItem.Name = itemDto.Name;
            existingItem.Description = itemDto.Description;
            existingItem.Price = itemDto.Price;

            _context.ItemTags.RemoveRange(existingItem.ItemTags);

            if (itemDto.TagIds != null && itemDto.TagIds.Any())
            {
                var existingTagIds = existingItem.ItemTags.Select(it => it.TagId).ToList();

                var newTagIds = itemDto.TagIds
                    .Where(tagId => !existingTagIds.Contains(tagId))
                    .ToList();

                if (newTagIds.Any())
                {
                    var itemTags = newTagIds.Select(tagId => new ItemTag
                    {
                        ItemId = existingItem.Id,
                        TagId = tagId
                    }).ToList();

                    _context.ItemTags.AddRange(itemTags);
                }
            }

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

        public async Task<IEnumerable<Item>> GetFilteredItemsAsync(ItemFilter filter)
        {
            var query = _context.Items
                .Include(i => i.ItemTags)
                    .ThenInclude(it => it.Tag)
                .Include(i => i.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(i =>
                    i.Name.ToLower().Contains(searchTerm) ||
                    i.Description.ToLower().Contains(searchTerm));
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(i => i.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(i => i.Price <= filter.MaxPrice.Value);
            }

            if (filter.MinStock.HasValue)
            {
                query = query.Where(i => i.StockQuantity >= filter.MinStock.Value);
            }

            if (filter.MinRating.HasValue)
            {
                query = query.Where(i => i.Reviews.Average(r => r.Rating) >= filter.MinRating.Value);
            }

            if (filter.Tags != null && filter.Tags.Any())
            {
                query = query.Where(i => i.ItemTags.Any(t => filter.Tags.Contains(t.Tag.Name)));
            }

            if (filter.HasDiscount.HasValue)
            {
                query = query.Where(i =>
                    filter.HasDiscount.Value ?
                    i.OldPrice.HasValue && i.OldPrice > i.Price :
                    !i.OldPrice.HasValue || i.OldPrice <= i.Price);
            }

            //if (!string.IsNullOrEmpty(filter.SortBy))
            //{
            //    query = filter.SortBy.ToLower() switch
            //    {
            //        "price" => filter.SortDescending ?
            //            query.OrderByDescending(i => i.Price) :
            //            query.OrderBy(i => i.Price),
            //        "name" => filter.SortDescending ?
            //            query.OrderByDescending(i => i.Name) :
            //            query.OrderBy(i => i.Name),
            //        "rating" => filter.SortDescending ?
            //            query.OrderByDescending(i => i.Reviews.Average(r => r.Rating)) :
            //            query.OrderBy(i => i.Reviews.Average(r => r.Rating)),
            //        "stock" => filter.SortDescending ?
            //            query.OrderByDescending(i => i.StockQuantity) :
            //            query.OrderBy(i => i.StockQuantity),
            //        _ => query.OrderBy(i => i.Id)
            //    };
            //}

            return await query.ToListAsync();
        }
    }
}
