using tai_shop.Dtos.Review;
using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllAsync();
        Task<Review> CreateReviewAsync(string userId, CreateReviewDto reviewDto);
        Task<IEnumerable<Review>> GetProductReviewsAsync(int productId);
        Task<double> GetProductAverageRatingAsync(int productId);
    }
}
