using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Relations;
using Server.DTOs.Review;
using Server.Repository.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IServerReviewRepository _repository;

    public ReviewsController(IServerReviewRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ReviewListDto>>> GetReviews()
    {
        try
        {
            return Ok(await _repository.GetReviewsAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{reviewId}", Name = "GetReview")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReviewReadDto>> GetReview(int reviewId)
    {
        try
        {
            return Ok(await _repository.GetReviewAsync(reviewId));
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Review with ID {reviewId} not found.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("book/{bookId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ReviewsOfABook>>> GetReviewsOfABook(int bookId)
    {
        try
        {
            return Ok(await _repository.GetReviewsOfABookAsync(bookId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{reviewId}/book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BookOfAReview>> GetBookOfAReview(int reviewId)
    {
        try
        {
            return Ok(await _repository.GetBookOfAReviewAsync(reviewId));
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Book not found for review with ID {reviewId}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReviewReadDto>> CreateReview([FromBody] ReviewCreateDto reviewCreateDto)
    {
        if (reviewCreateDto == null)
        {
            return BadRequest("Review data cannot be null.");
        }

        try
        {
            var createdReview = await _repository.CreateReviewAsync(reviewCreateDto);
            return CreatedAtRoute(nameof(GetReview), new { reviewId = createdReview.Id }, createdReview);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateReview(int reviewId, [FromBody] ReviewUpdateDto reviewUpdateDto)
    {
        if (reviewUpdateDto == null || reviewId != reviewUpdateDto.Id)
        {
            return BadRequest("Invalid review data.");
        }

        try
        {
            var updated = await _repository.UpdateReviewAsync(reviewId, reviewUpdateDto);
            if (!updated)
            {
                return NotFound($"Review with ID {reviewId} not found.");
            }

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteReview(int reviewId)
    {
        try
        {
            var deleted = await _repository.DeleteReviewAsync(reviewId);
            if (!deleted)
            {
                return NotFound($"Review with ID {reviewId} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteReviews([FromBody] List<int> reviewIds)
    {
        if (reviewIds == null || !reviewIds.Any())
        {
            return BadRequest("Review IDs cannot be null or empty.");
        }

        try
        {
            var deleted = await _repository.DeleteReviewsAsync(reviewIds);
            if (!deleted)
            {
                return NotFound("No reviews found for the provided IDs.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
