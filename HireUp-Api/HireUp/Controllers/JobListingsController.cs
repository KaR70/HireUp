using HireUp.Database.Interfaces;
using HireUp.DTOs.JobListing;
using HireUp.Entities; // تم إضافة هذا السطر ليتعرف على SavedJob
using HireUp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HireUp.Controllers;

[ApiController]
[Route("api/joblistings")]
[Produces("application/json")]
public class JobListingsController : ControllerBase
{
    private readonly ISavedJobRepository _savedJobRepository;
    private readonly IJobListingService _jobListingService;

    // تم تعديل الـ Constructor لاستقبال الـ Repository وحل مشكلة الـ NullReferenceException
    public JobListingsController(IJobListingService jobListingService, ISavedJobRepository savedJobRepository)
    {
        _jobListingService = jobListingService;
        _savedJobRepository = savedJobRepository;
    }

    
    [HttpGet("featured")]
    [ProducesResponseType(typeof(IEnumerable<JobListingSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeaturedJobs()
    {
        var result = await _jobListingService.GetFeaturedAsync();

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    
    [HttpGet("popular")]
    [ProducesResponseType(typeof(IEnumerable<JobListingSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPopularJobs()
    {
        var result = await _jobListingService.GetPopularAsync();

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(JobListingDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobById(int id)
    {
        var result = await _jobListingService.GetByIdAsync(id);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("{id}/save")]
    public async Task<IActionResult> SaveJob(int id, [FromQuery] string userId)
    {
        // التأكد من أن الوظيفة غير محفوظة مسبقاً لهذا المستخدم
        if (await _savedJobRepository.IsAlreadySavedAsync(id, userId))
            return BadRequest("هذه الوظيفة محفوظة بالفعل في قائمتك.");

        var savedJob = new SavedJob
        {
            JobListingId = id,
            UserId = userId,
            SavedAt = DateTime.UtcNow
        };

        await _savedJobRepository.AddAsync(savedJob);
        return Ok(new { message = "تم حفظ الوظيفة بنجاح" });
    }

    
    [HttpDelete("{id}/unsave")]
    public async Task<IActionResult> UnsaveJob(int id, [FromQuery] string userId)
    {
        await _savedJobRepository.RemoveAsync(id, userId);
        return Ok(new { message = "تمت إزالة الوظيفة من المحفوظات" });
    }
}