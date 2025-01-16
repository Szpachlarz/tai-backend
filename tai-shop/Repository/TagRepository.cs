using Microsoft.EntityFrameworkCore;
using tai_shop.Data;
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

        public async Task<Tag> AddTagAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
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

        public async Task<Tag?> UpdateTagAsync(int tagId, Tag tag)
        {
            var existingTag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == tagId);

            if (existingTag == null)
            {
                return null;
            }

            existingTag.Name = tag.Name;

            await _context.SaveChangesAsync();

            return existingTag;
        }
    }
}
