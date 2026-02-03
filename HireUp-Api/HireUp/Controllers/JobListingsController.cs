using HireUp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HireUp.Controllers;

[ApiController]
[Route("api/joblistings")]
public class JobListingsController : ControllerBase
{
    private readonly IJobListingService _jobListingService;

    public JobListingsController(IJobListingService jobListingService)
    {
        _jobListingService = jobListingService;
    }
    
    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedJobs()
    {
        var result = await _jobListingService.GetFeaturedAsync();

        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
    
    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularJobs()
    {
        var result = await _jobListingService.GetPopularAsync();
        
        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(int id)
    {
        var result = await _jobListingService.GetByIdAsync(id);
        
        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
}