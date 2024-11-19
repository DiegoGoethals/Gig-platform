using Gig.Platform.Core.Interfaces.Repositories;
using Gig.Platform.Core.Interfaces.Services;
using Gig.Platform.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository<ApplicationUser> _userRepository;

        public ReviewService(IReviewRepository reviewRepository, IUserRepository<ApplicationUser> userRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        public async Task<ResultModel<Review>> CreateAsync(int rating, string comment, Guid reviewerId, Guid revieweeId)
        {
            var reviewer = await _userRepository.GetByIdAsync(reviewerId);
            var review = new Review
            {
                Id = Guid.NewGuid(),
                Rating = rating,
                Comment = comment,
                ReviewerId = reviewerId,
                RevieweeId = revieweeId,
                Reviewer = reviewer
            };

            if (await _reviewRepository.AddAsync(review))
            {
                return new ResultModel<Review>
                {
                    Value = review,
                    IsSucces = true
                };
            }
            return new ResultModel<Review>
            {
                IsSucces = false,
                Errors = new List<string> { "Error creating Review" }
            };
        }

        public async Task<ResultModel<Review>> UpdateAsync(Guid id, int rating, string comment)
        {
            var review = await _reviewRepository.GetByIdAsync(id);

            if (review == null)
            {
                return new ResultModel<Review>
                {
                    IsSucces = false,
                    Errors = new List<string> { "Review not found" }
                };
            }

            review.Rating = rating;
            review.Comment = comment;

            if (await _reviewRepository.UpdateAsync(review))
            {
                return new ResultModel<Review>
                {
                    Value = review,
                    IsSucces = true
                };
            }
            return new ResultModel<Review>
            {
                IsSucces = false,
                Errors = new List<string> { "Error updating Review" }
            };
        }

        public async Task<ResultModel<Review>> DeleteAsync(Guid id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);

            if (review == null)
            {
                return new ResultModel<Review>
                {
                    IsSucces = false,
                    Errors = new List<string> { "Review not found" }
                };
            }

            if (await _reviewRepository.DeleteAsync(review))
            {
                return new ResultModel<Review>
                {
                    IsSucces = true
                };
            }
            return new ResultModel<Review>
            {
                IsSucces = false,
                Errors = new List<string> { "Error deleting Review" }
            };
        }

        public async Task<ResultModel<IEnumerable<Review>>> GetAllByRevieweeIdAsync(Guid revieweeId)
        {
            var reviews = await _reviewRepository.GetByRevieweeIdAsync(revieweeId);

            if (reviews.Any())
            {
                return new ResultModel<IEnumerable<Review>>
                {
                    Value = reviews,
                    IsSucces = true
                };
            }
            return new ResultModel<IEnumerable<Review>>
            {
                IsSucces = true,
                Value = new List<Review>()
            };
        }

        public async Task<ResultModel<IEnumerable<Review>>> GetAllByReviewerIdAsync(Guid reviewerId)
        {
            var reviews = await _reviewRepository.GetByReviewerIdAsync(reviewerId);

            if (reviews.Any())
            {
                return new ResultModel<IEnumerable<Review>>
                {
                    Value = reviews,
                    IsSucces = true
                };
            }
            return new ResultModel<IEnumerable<Review>>
            {
                IsSucces = true,
                Value = new List<Review>()
            };
        }

        public async Task<ResultModel<Review>> GetByIdAsync(Guid id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);

            if (review != null)
            {
                return new ResultModel<Review>
                {
                    Value = review,
                    IsSucces = true
                };
            }
            return new ResultModel<Review>
            {
                IsSucces = false,
                Errors = new List<string> { "Review not found" }
            };
        }
    }
}
