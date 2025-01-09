using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment hosting;
        private readonly IUnitOfWork unitOfWork;

        public PhotoService(IConfiguration configuration, IHostEnvironment hosting, IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.hosting = hosting;
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<Photo>> Upload(Item item, IEnumerable<IFormFile> files)
        {
            var uploadsFolderName = configuration.GetSection("FileUpload:Folder").Value;
            var uploadsFolderPath = Path.Combine(hosting.ContentRootPath, uploadsFolderName);

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var photos = new List<Photo>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    throw new InvalidOperationException("Unsupported file type.");
                }

                if (file.Length > 5 * 1024 * 1024) // przykładowy limit
                {
                    throw new InvalidOperationException("File size exceeds the 5 MB limit.");
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var photo = new Photo()
                {
                    ItemId = item.Id,
                    Filepath = Path.Combine(uploadsFolderName, fileName),
                    Filename = fileName,
                    Length = file.Length
                };

                photos.Add(photo);
                item.Photos.Add(photo);
            }
            
            await unitOfWork.CompleteAsync();
            return photos;
        }

        public bool Delete(string fileName)
        {
            var uploadsFolderName = configuration.GetSection("FileUpload:Folder").Value;
            var uploadsFolderPath = Path.Combine(hosting.ContentRootPath, uploadsFolderName);

            var filePath = Path.Combine(uploadsFolderPath, fileName);

            if ((System.IO.File.Exists(filePath)))
            {
                System.IO.File.Delete(filePath);

                return true;
            }

            return false;
        }

    }
}
