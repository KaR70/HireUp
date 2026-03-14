using HireUp.Database;
using HireUp.DTOs;
using HireUp.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace HireUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public JobApplicationsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("apply-job")]
        public async Task<IActionResult> Apply([FromForm] JobApplicationCreateDto dto, [FromForm] string userId)
        {
            try
            {
                // 1. التأكد من وجود الملف
                if (dto.ResumeFile == null || dto.ResumeFile.Length == 0)
                    return BadRequest("ملف السيرة الذاتية مطلوب.");

                // 2. تحديد مسار الـ wwwroot بدقة
                var projectRoot = Directory.GetCurrentDirectory();
                var uploadsPath = Path.Combine(projectRoot, "wwwroot", "uploads", "resumes");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // 3. توليد اسم فريد للملف وحفظه
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ResumeFile.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ResumeFile.CopyToAsync(stream);
                }

                // 4. حفظ السجل في الداتابيز بالـ UserId الحقيقي
                var application = new JobApplication
                {
                    JobListingId = dto.JobListingId,
                    JobSeekerId = userId, // هنستخدم الـ ID اللي هتحطيه في Swagger
                    CoverLetter = dto.CoverLetter,
                    ResumeUrl = "/uploads/resumes/" + fileName,
                    Status = ApplicationStatus.Pending,
                    AppliedAt = DateTime.UtcNow
                };

                _context.JobApplications.Add(application);
                await _context.SaveChangesAsync();

                return Ok(new { message = "تم التقديم وحفظ الملف بنجاح!", fileName });
            }
            catch (Exception ex)
            {
                // لو حصل مشكلة هيطلعلك تفصيل الخطأ بدل 500 مجهولة
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"خطأ في الداتابيز: {message}");
            }
        }

        [HttpGet("track-my-apps/{userId}")]
        public async Task<IActionResult> GetMyApplications(string userId)
        {
            var apps = await _context.JobApplications
                .Where(a => a.JobSeekerId == userId)
                .Include(a => a.JobListing)
                .Select(a => new {
                    a.Id,
                    JobTitle = a.JobListing.Title,
                    Status = a.Status.ToString(),
                    a.AppliedAt,
                    a.ResumeUrl
                })
                .ToListAsync();

            return Ok(apps);
        }
    }
}