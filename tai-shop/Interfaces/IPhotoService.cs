using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IPhotoService
    {
        Task<List<Photo>> Upload(Item item, IEnumerable<IFormFile> files);
        bool Delete(string fileName);
    }
}
