using Microsoft.EntityFrameworkCore;
using tai_shop.Data;
using tai_shop.Dtos;
using tai_shop.Dtos.Item;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> CreateTagAsync(TagDto tagDto)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagDto.Name);

            if (tag == null)
            {
                tag = new Tag { Name = tagDto.Name };
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            return tag;
        }

        public async Task<Tag?> DeleteTagAsync(int tagId)
        {
            var tag = await GetTagByIdAsync(tagId);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
            }
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetTagByIdAsync(int tagId)
        {
            return await _context.Tags.FirstOrDefaultAsync(o => o.Id == tagId);
        }

        public async Task<Tag?> UpdateTagAsync(int tagId, TagDto tagDto)
        {
            var existingTag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == tagId);

            if (existingTag == null)
            {
                return null;
            }

            existingTag.Name = tagDto.Name;

            await _context.SaveChangesAsync();

            return existingTag;
        }

        public async Task AddTagToProductAsync(int itemId, int tagId)
        {
            var itemTag = new ItemTag
            {
                ItemId = itemId,
                TagId = tagId
            };

            _context.ItemTags.Add(itemTag);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetItemTagsAsync(int itemId)
        {
            return await _context.ItemTags
                .Where(p => p.ItemId == itemId)
                .Select(p => p.Tag)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsByTagAsync(string tagName)
        {
            return await _context.ItemTags
                .Where(pt => pt.Tag.Name == tagName)
                .Select(pt => pt.Item)
                .ToListAsync();
        }

        public async Task RemoveTagFromItemAsync(int itemId, int tagId)
        {
            var itemTag = await _context.ItemTags
                .FirstOrDefaultAsync(pt => pt.ItemId == itemId && pt.TagId == tagId);

            if (itemTag != null)
            {
                _context.ItemTags.Remove(itemTag);
                await _context.SaveChangesAsync();
            }
        }
    }
}
