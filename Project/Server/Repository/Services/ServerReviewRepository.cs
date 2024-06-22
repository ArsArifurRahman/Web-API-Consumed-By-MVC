using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs.Relations;
using Server.DTOs.Review;
using Server.Models;
using Server.Repository.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Server.Repository;

public class ServerReviewRepository : IServerReviewRepository
{
    private readonly DataContext _context;

    public ServerReviewRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<ReviewListDto>> GetReviewsAsync()
    {
        try
        {
            return await _context.Reviews
                .Select(r => new ReviewListDto
                {
                    Headline = r.Headline,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error retrieving reviews.", ex);
        }
    }

    public async Task<ReviewReadDto> GetReviewAsync(int reviewId)
    {
        try
        {
            var review = await _context.Reviews
                .Include(r => r.Book)
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {reviewId} not found.");
            }

            return new ReviewReadDto
            {
                Id = review.Id,
                Headline = review.Headline,
                ReviewText = review.ReviewText,
                Rating = review.Rating,
                BookId = review.BookId,
                BookTitle = review.Book?.Title ?? string.Empty,
                ReviewerId = review.ReviewerId,
                ReviewerName = $"{review.Reviewer?.FirstName} {review.Reviewer?.LastName}" ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving review with ID {reviewId}.", ex);
        }
    }

    public async Task<IEnumerable<ReviewsOfABook>> GetReviewsOfABookAsync(int bookId)
    {
        try
        {
            var reviews = await _context.Reviews
                .Where(r => r.BookId == bookId)
                .Select(r => new ReviewsOfABook
                {
                    Id = r.Id,
                    Headline = r.Headline,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating,
                    ReviewerId = r.ReviewerId,
                    ReviewerFullName = $"{r.Reviewer!.FirstName} {r.Reviewer.LastName}"
                })
                .ToListAsync();

            return reviews;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving reviews for book with ID {bookId}.", ex);
        }
    }

    public async Task<BookOfAReview> GetBookOfAReviewAsync(int reviewId)
    {
        try
        {
            var review = await _context.Reviews
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null || review.Book == null)
            {
                throw new KeyNotFoundException($"Book not found for review with ID {reviewId}");
            }

            return new BookOfAReview
            {
                Id = review.Book.Id,
                Isbn = review.Book.Isbn,
                Title = review.Book.Title,
                PublishedAt = review.Book.PublishedAt,
                ReviewId = review.Id
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving book for review with ID {reviewId}.", ex);
        }
    }

    public async Task<bool> ReviewExistsAsync(int reviewId)
    {
        try
        {
            return await _context.Reviews.AnyAsync(r => r.Id == reviewId);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error checking existence of review with ID {reviewId}.", ex);
        }
    }

    public async Task<ReviewReadDto> CreateReviewAsync(ReviewCreateDto reviewCreateDto)
    {
        if (reviewCreateDto == null)
        {
            throw new ArgumentNullException(nameof(reviewCreateDto), "Review data cannot be null.");
        }

        ValidateReviewCreateDto(reviewCreateDto);

        var review = new Review
        {
            Headline = reviewCreateDto.Headline,
            ReviewText = reviewCreateDto.ReviewText,
            Rating = reviewCreateDto.Rating,
            BookId = reviewCreateDto.BookId,
            ReviewerId = reviewCreateDto.ReviewerId
        };

        try
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return new ReviewReadDto
            {
                Id = review.Id,
                Headline = review.Headline,
                ReviewText = review.ReviewText,
                Rating = review.Rating,
                BookId = review.BookId,
                BookTitle = (await _context.Books.FindAsync(review.BookId))?.Title ?? string.Empty,
                ReviewerId = review.ReviewerId,
                ReviewerName = (await _context.Reviewers.FindAsync(review.ReviewerId))?.FirstName + " " + (await _context.Reviewers.FindAsync(review.ReviewerId))?.LastName ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error creating review.", ex);
        }
    }

    public async Task<bool> UpdateReviewAsync(int reviewId, ReviewUpdateDto reviewUpdateDto)
    {
        if (reviewUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(reviewUpdateDto), "Review update data cannot be null.");
        }

        ValidateReviewUpdateDto(reviewUpdateDto);

        try
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                return false;
            }

            review.Headline = reviewUpdateDto.Headline;
            review.ReviewText = reviewUpdateDto.ReviewText;
            review.Rating = reviewUpdateDto.Rating;

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error updating review with ID {reviewId}.", ex);
        }
    }

    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        try
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                return false;
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error deleting review with ID {reviewId}.", ex);
        }
    }

    public async Task<bool> DeleteReviewsAsync(List<int> reviewIds)
    {
        try
        {
            var reviews = await _context.Reviews.Where(r => reviewIds.Contains(r.Id)).ToListAsync();

            if (!reviews.Any())
            {
                return false;
            }

            _context.Reviews.RemoveRange(reviews);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error deleting reviews.", ex);
        }
    }

    private void ValidateReviewCreateDto(ReviewCreateDto reviewCreateDto)
    {
        var context = new ValidationContext(reviewCreateDto, serviceProvider: null, items: null);
        Validator.ValidateObject(reviewCreateDto, context, validateAllProperties: true);
    }

    private void ValidateReviewUpdateDto(ReviewUpdateDto reviewUpdateDto)
    {
        var context = new ValidationContext(reviewUpdateDto, serviceProvider: null, items: null);
        Validator.ValidateObject(reviewUpdateDto, context, validateAllProperties: true);
    }
}
