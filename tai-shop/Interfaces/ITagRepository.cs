using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int tagId);
        Task<Tag> AddTagAsync(Tag tag);
        Task<Tag> UpdateTagAsync(int tagId, Tag tag);
        Task<Tag> DeleteTagAsync(int tagId);
    }
}
