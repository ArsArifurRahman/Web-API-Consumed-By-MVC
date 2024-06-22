using Server.DTOs.Relations;
using Server.DTOs.Review;

namespace Server.Repository.Contracts;

public interface IServerReviewRepository
{
    Task<IEnumerable<ReviewListDto>> GetReviewsAsync();
    Task<ReviewReadDto> GetReviewAsync(int reviewId);
    Task<IEnumerable<ReviewsOfABook>> GetReviewsOfABookAsync(int bookId);
    Task<BookOfAReview> GetBookOfAReviewAsync(int reviewId);
    Task<bool> ReviewExistsAsync(int reviewId);
    Task<ReviewReadDto> CreateReviewAsync(ReviewCreateDto reviewCreateDto);
    Task<bool> UpdateReviewAsync(int reviewId, ReviewUpdateDto reviewUpdateDto);
    Task<bool> DeleteReviewAsync(int reviewId);
    Task<bool> DeleteReviewsAsync(List<int> reviewId);
}