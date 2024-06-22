using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Relations;
using Server.DTOs.Reviewer;
using Server.Repository.Contracts;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewersController : ControllerBase
{
    private readonly IServerReviewerRepository _repository;

    public ReviewersController(IServerReviewerRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReviewerListDto>), 200)]
    public async Task<IActionResult> GetReviewers()
    {
        return Ok(await _repository.GetReviewersAsync());
    }

    [HttpGet("{reviewerId}", Name = "GetReviewer")]
    [ProducesResponseType(typeof(ReviewerReadDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetReviewer(int reviewerId)
    {
        try
        {
            return Ok(await _repository.GetReviewerAsync(reviewerId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{reviewerId}/reviews")]
    [ProducesResponseType(typeof(IEnumerable<ReviewsOfAReviewer>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetReviewsOfAReviewer(int reviewerId)
    {
        try
        {
            return Ok(await _repository.GetReviewsOfAReviewerAsync(reviewerId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("review/{reviewId}/reviewer")]
    [ProducesResponseType(typeof(ReviewerOfAReview), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetReviewerOfAReview(int reviewId)
    {
        try
        {
            return Ok(await _repository.GetReviewerOfAReviewAsync(reviewId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReviewerReadDto), 201)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> CreateReviewer(ReviewerCreateDto reviewerCreateDto)
    {
        try
        {
            var createdReviewer = await _repository.CreateReviewerAsync(reviewerCreateDto);
            return CreatedAtRoute(nameof(GetReviewer), new { reviewerId = createdReviewer.Id }, createdReviewer);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{reviewerId}")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> UpdateReviewer(int reviewerId, ReviewerUpdateDto reviewerUpdateDto)
    {
        try
        {
            var updated = await _repository.UpdateReviewerAsync(reviewerId, reviewerUpdateDto);
            if (!updated)
            {
                return NotFound($"Reviewer with ID {reviewerId} not found.");
            }
            return NoContent();
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{reviewerId}")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> DeleteReviewer(int reviewerId)
    {
        var deleted = await _repository.DeleteReviewerAsync(reviewerId);
        if (!deleted)
        {
            return NotFound($"Reviewer with ID {reviewerId} not found.");
        }
        return NoContent();
    }
}