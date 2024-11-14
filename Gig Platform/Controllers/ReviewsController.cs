using Gig.Platform.Core.Interfaces.Services;
using Gig_Platform.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gig_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private IActionResult HandleError(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
            return BadRequest(ModelState.Values);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewRequestDto reviewRequestDto)
        {
            var result = await _reviewService.CreateAsync(reviewRequestDto.Rating, reviewRequestDto.Comment, reviewRequestDto.ReviewerId, reviewRequestDto.RevieweeId);
            if (result.IsSucces)
            {
                return CreatedAtAction(nameof(Create), new { result.Value.Id }, new ReviewResponseDto
                {
                    Id = result.Value.Id,
                    Rating = result.Value.Rating,
                    Comment = result.Value.Comment,
                    ReviewerId = result.Value.ReviewerId,
                    RevieweeId = result.Value.RevieweeId,
                    ReviewerName = result.Value.Reviewer.UserName
                });
            }
            return HandleError(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ReviewRequestDto reviewRequestDto)
        {
            var result = await _reviewService.UpdateAsync(id, reviewRequestDto.Rating, reviewRequestDto.Comment);
            if (result.IsSucces)
            {
                return Ok(new ReviewResponseDto
                {
                    Id = result.Value.Id,
                    Rating = result.Value.Rating,
                    Comment = result.Value.Comment,
                    ReviewerId = result.Value.ReviewerId,
                    RevieweeId = result.Value.RevieweeId
                });
            }
            return HandleError(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _reviewService.DeleteAsync(id);
            if (result.IsSucces)
            {
                return Ok("Review deleted succesfully!");
            }
            return HandleError(result.Errors);
        }

        [HttpGet("reviewee/{revieweeId}")]
        public async Task<IActionResult> GetAllByRevieweeId(Guid revieweeId)
        {
            var result = await _reviewService.GetAllByRevieweeIdAsync(revieweeId);
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(review => new ReviewResponseDto
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ReviewerId = review.ReviewerId,
                    RevieweeId = review.RevieweeId
                }));
            }
            return HandleError(result.Errors);
        }

        [HttpGet("reviewer/{reviewerId}")]
        public async Task<IActionResult> GetAllByReviewerId(Guid reviewerId)
        {
            var result = await _reviewService.GetAllByReviewerIdAsync(reviewerId);
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(review => new ReviewResponseDto
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ReviewerId = review.ReviewerId,
                    RevieweeId = review.RevieweeId
                }));
            }
            return HandleError(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reviewService.GetByIdAsync(id);
            if (result.IsSucces)
            {
                return Ok(new ReviewResponseDto
                {
                    Id = result.Value.Id,
                    Rating = result.Value.Rating,
                    Comment = result.Value.Comment,
                    ReviewerId = result.Value.ReviewerId,
                    RevieweeId = result.Value.RevieweeId
                });
            }
            return HandleError(result.Errors);
        }
    }
}
