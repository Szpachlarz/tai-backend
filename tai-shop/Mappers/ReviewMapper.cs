using tai_shop.Dtos.Review;
using tai_shop.Models;

namespace tai_shop.Mappers
{
    public static class ReviewMapper
    {
        public static ReviewDto ToDto(this Review review)
        {
            return new ReviewDto
            {
                Id = review.Id,
                ItemId = review.ItemId,
                Rating = review.Rating,
                UserId = review.UserId,
                FirstName = review.User.FirstName,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public static IEnumerable<ReviewDto> ToDtoList(this IEnumerable<Review> reviews)
        {
            return reviews.Select(r => r.ToDto()).ToList();
        }
    }
}
