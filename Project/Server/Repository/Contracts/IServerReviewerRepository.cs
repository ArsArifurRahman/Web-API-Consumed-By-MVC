using Server.DTOs.Relations;
using Server.DTOs.Reviewer;

namespace Server.Repository.Contracts;

public interface IServerReviewerRepository
{
    Task<IEnumerable<ReviewerListDto>> GetReviewersAsync();
    Task<ReviewerReadDto> GetReviewerAsync(int reviewerId);
    Task<IEnumerable<ReviewsOfAReviewer>> GetReviewsOfAReviewerAsync(int reviewerId);
    Task<ReviewerOfAReview> GetReviewerOfAReviewAsync(int reviewId);
    Task<bool> ReviewerExistsAsync(int reviewerId);
    Task<ReviewerReadDto> CreateReviewerAsync(ReviewerCreateDto reviewerCreateDto);
    Task<bool> UpdateReviewerAsync(int reviewerId, ReviewerUpdateDto reviewerUpdateDto);
    Task<bool> DeleteReviewerAsync(int reviewerId);
}
