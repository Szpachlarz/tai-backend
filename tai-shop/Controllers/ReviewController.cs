using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using tai_shop.Dtos.Review;
using tai_shop.Interfaces;
using tai_shop.Models;
using tai_shop.Repository;
using tai_shop.Mappers;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IItemRepository _itemRepository;
        private readonly UserManager<AppUser> _userManager;
        public ReviewController(IReviewRepository reviewRepository, UserManager<AppUser> userManager, IItemRepository itemRepository)
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _itemRepository = itemRepository;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviews = await _reviewRepository.GetAllAsync();

            return Ok(reviews.ToDtoList());
        }

        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(CreateReviewDto reviewDto)
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
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetProductReviews(int itemId)
        {
            if (itemId <= 0)
            {
                return BadRequest("Invalid item ID.");
            }

            var itemExists = await _itemRepository.ItemExistsAsync(itemId);
            if (!itemExists)
            {
                return NotFound("Item not found.");
            }

            var reviews = await _reviewRepository.GetProductReviewsAsync(itemId);

            return Ok(reviews.ToDtoList());
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
