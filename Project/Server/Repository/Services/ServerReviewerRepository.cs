using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs.Relations;
using Server.DTOs.Reviewer;
using Server.Models;
using Server.Repository.Contracts;

public class ServerReviewerRepository : IServerReviewerRepository
{
    private readonly DataContext _context;

    public ServerReviewerRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<ReviewerListDto>> GetReviewersAsync()
    {
        return await _context.Reviewers
            .Select(r => new ReviewerListDto
            {
                FirstName = r.FirstName,
                LastName = r.LastName
            })
            .ToListAsync();
    }

    public async Task<ReviewerReadDto> GetReviewerAsync(int reviewerId)
    {
        var reviewer = await _context.Reviewers
            .Include(r => r.Reviews)
            .FirstOrDefaultAsync(r => r.Id == reviewerId);

        if (reviewer == null)
        {
            throw new KeyNotFoundException($"Reviewer with ID {reviewerId} not found.");
        }

        return new ReviewerReadDto
        {
            Id = reviewer.Id,
            FirstName = reviewer.FirstName,
            LastName = reviewer.LastName
        };
    }

    public async Task<IEnumerable<ReviewsOfAReviewer>> GetReviewsOfAReviewerAsync(int reviewerId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.ReviewerId == reviewerId)
            .Select(r => new ReviewsOfAReviewer
            {
                Id = r.Id,
                Headline = r.Headline,
                ReviewText = r.ReviewText,
                Rating = r.Rating,
                BookId = r.BookId
            })
            .ToListAsync();

        if (reviews == null || reviews.Count == 0)
        {
            throw new KeyNotFoundException($"No reviews found for reviewer with ID {reviewerId}.");
        }

        return reviews;
    }

    public async Task<ReviewerOfAReview> GetReviewerOfAReviewAsync(int reviewId)
    {
        var review = await _context.Reviews
            .Include(r => r.Reviewer)
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        if (review == null || review.Reviewer == null)
        {
            throw new KeyNotFoundException($"Reviewer not found for review with ID {reviewId}");
        }

        return new ReviewerOfAReview
        {
            Id = review.Reviewer.Id,
            FirstName = review.Reviewer.FirstName,
            LastName = review.Reviewer.LastName,
            ReviewId = review.Id
        };
    }

    public async Task<bool> ReviewerExistsAsync(int reviewerId)
    {
        return await _context.Reviewers.AnyAsync(r => r.Id == reviewerId);
    }

    public async Task<ReviewerReadDto> CreateReviewerAsync(ReviewerCreateDto reviewerCreateDto)
    {
        if (reviewerCreateDto == null)
        {
            throw new ArgumentNullException(nameof(reviewerCreateDto), "Reviewer data cannot be null.");
        }

        var reviewer = new Reviewer
        {
            FirstName = reviewerCreateDto.FirstName,
            LastName = reviewerCreateDto.LastName
        };

        _context.Reviewers.Add(reviewer);
        await _context.SaveChangesAsync();

        return new ReviewerReadDto
        {
            Id = reviewer.Id,
            FirstName = reviewer.FirstName,
            LastName = reviewer.LastName
        };
    }

    public async Task<bool> UpdateReviewerAsync(int reviewerId, ReviewerUpdateDto reviewerUpdateDto)
    {
        if (reviewerUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(reviewerUpdateDto), "Reviewer update data cannot be null.");
        }

        var reviewer = await _context.Reviewers.FindAsync(reviewerId);

        if (reviewer == null)
        {
            return false;
        }

        reviewer.FirstName = reviewerUpdateDto.FirstName;
        reviewer.LastName = reviewerUpdateDto.LastName;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteReviewerAsync(int reviewerId)
    {
        var reviewer = await _context.Reviewers.FindAsync(reviewerId);

        if (reviewer == null)
        {
            return false;
        }

        _context.Reviewers.Remove(reviewer);
        await _context.SaveChangesAsync();

        return true;
    }
}
