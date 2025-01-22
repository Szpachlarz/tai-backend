using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using tai_shop.Dtos;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<AppUser> _userManager;
        public ReviewController(IReviewRepository reviewRepository, UserManager<AppUser> userManager)
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(ReviewDto reviewDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var review = await _reviewRepository.CreateReviewAsync(userId, reviewDto);
                return CreatedAtAction(nameof(GetProductReviews), new { itemId = review.ItemId }, review);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("item/{itemId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Review>>> GetProductReviews(int itemId)
        {
            var reviews = await _reviewRepository.GetProductReviewsAsync(itemId);
            return Ok(reviews);
        }

        [HttpGet("item/{itemId}/rating")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> GetProductAverageRating(int itemId)
        {
            var rating = await _reviewRepository.GetProductAverageRatingAsync(itemId);
            return Ok(rating);
        }
    }
}
