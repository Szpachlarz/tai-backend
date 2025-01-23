using tai_shop.Dtos;
using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int tagId);
        Task<Tag> CreateTagAsync(TagDto tagDto);
        Task<Tag> UpdateTagAsync(int tagId, Tag tag);
        Task<Tag> DeleteTagAsync(int tagId);
        Task AddTagToProductAsync(int itemId, int tagId);
        Task<IEnumerable<Tag>> GetItemTagsAsync(int itemId);
        Task<IEnumerable<Item>> GetItemsByTagAsync(string tagName);
        Task RemoveTagFromProductAsync(int itemId, int tagId);
    }
}
