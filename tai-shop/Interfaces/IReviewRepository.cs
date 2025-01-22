using tai_shop.Dtos;
using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> CreateReviewAsync(string userId, ReviewDto reviewDto);
        Task<IEnumerable<Review>> GetProductReviewsAsync(int productId);
        Task<double> GetProductAverageRatingAsync(int productId);
    }
}
