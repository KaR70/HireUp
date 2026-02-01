using Microsoft.AspNetCore.Mvc;
using HireUp.DTOs.Authentication;
using HireUp.Services;

namespace HireUp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MockInterviewController : ControllerBase
{
    private readonly IMockInterviewService _service;

    public MockInterviewController(IMockInterviewService service)
    {
        _service = service;
    }

    // GET: api/MockInterview
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var interviews = await _service.GetAllAsync();
        return Ok(interviews);
    }

    // GET: api/MockInterview/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var interview = await _service.GetByIdAsync(id);
        if (interview == null)
            return NotFound("Interview not found");

        return Ok(interview);
    }

    // POST: api/MockInterview
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MockInterviewDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/MockInterview/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MockInterviewDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            return NotFound("Interview not found");

        return NoContent();
    }

    // DELETE: api/MockInterview/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound("Interview not found");

        return NoContent();
    }
}
