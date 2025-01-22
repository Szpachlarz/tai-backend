using Microsoft.EntityFrameworkCore;
using tai_shop.Data;
using tai_shop.Dtos;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;        
        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateReviewAsync(string userId, ReviewDto reviewDto)
        {
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ItemId == reviewDto.ItemId && r.UserId == userId);

            if (existingReview != null)
            {
                throw new InvalidOperationException("User has already reviewed this product");
            }

            var review = new Review
            {
                ItemId = reviewDto.ItemId,
                UserId = userId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<IEnumerable<Review>> GetProductReviewsAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ItemId == productId)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetProductAverageRatingAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ItemId == productId)
                .AverageAsync(r => r.Rating);
        }
    }
}
