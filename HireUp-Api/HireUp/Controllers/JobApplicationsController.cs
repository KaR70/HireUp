using HireUp.Database;
using HireUp.DTOs;
using HireUp.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HireUp.Controllers
{
    /// <summary>
    /// Controller to handle job applications: submitting applications with resume upload and tracking user's applications.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class JobApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public JobApplicationsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// Apply to a job listing with a cover letter and a resume file (multipart/form-data).
        /// </summary>
        /// <remarks>
        /// Sample request (multipart/form-data):
        /// 
        /// POST /api/JobApplications/apply-job
        /// Content-Type: multipart/form-data
        /// Form Data:
        /// - jobListingId:123
        /// - coverLetter: "I am excited to apply for this position..."
        /// - resumeFile: (file upload - PDF or DOCX)
        /// - userId: "user-1"
        /// 
        /// Sample success response (200):
        /// 
        /// {
        /// "message": "تم التقديم وحفظ الملف بنجاح!",
        /// "fileName": "e7b5f6b3-9c3a-4d1f-8a2b-0f1c2d3e4a5b.pdf"
        /// }
        /// 
        /// Sample error response (400 - missing file):
        /// 
        /// "ملف السيرة الذاتية مطلوب."
        /// 
        /// Sample error response (500 - database/other error):
        /// 
        /// "خطأ في الداتابيز: <error message>"
        /// </remarks>
        /// <param name="dto">Application DTO containing JobListingId, CoverLetter and ResumeFile (IFormFile)</param>
        /// <param name="userId">The id of the applying user (provided in form data for testing via Swagger)</param>
        /// <returns>Returns a success message and the saved filename on success</returns>
        [HttpPost("apply-job")]
        [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]        
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Apply([FromForm] JobApplicationCreateDto dto, [FromForm] string userId)
        {
            try
            {
                //1. التأكد من وجود الملف
                if (dto.ResumeFile == null || dto.ResumeFile.Length ==0)
                    return BadRequest("ملف السيرة الذاتية مطلوب.");

                //2. تحديد مسار الـ wwwroot بدقة
                var projectRoot = Directory.GetCurrentDirectory();
                var uploadsPath = Path.Combine(projectRoot, "wwwroot", "uploads", "resumes");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                //3. توليد اسم فريد للملف وحفظه
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ResumeFile.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ResumeFile.CopyToAsync(stream);
                }

                //4. حفظ السجل في الداتابيز بالـ UserId الحقيقي
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
                // لو حصل مشكلة هيطلعلك تفصيل الخطأ بدل500 مجهولة
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"خطأ في الداتابيز: {message}");
            }
        }

        /// <summary>
        /// Retrieve applications submitted by a specific user (track my applications).
        /// </summary>
        /// <remarks>
        /// Sample success response (200):
        /// 
        /// GET /api/JobApplications/track-my-apps/user-1
        /// [
        /// {
        /// "id":42,
        /// "jobTitle": "Senior C# Developer",
        /// "status": "Pending",
        /// "appliedAt": "2025-03-14T12:34:56Z",
        /// "resumeUrl": "/uploads/resumes/e7b5f6b3-9c3a-4d1f-8a2b-0f1c2d3e4a5b.pdf"
        /// }
        /// ]
        /// </remarks>
        /// <param name="userId">The id of the job seeker to fetch applications for</param>
        /// <returns>Returns a list of simplified application records (id, job title, status, appliedAt, resumeUrl)</returns>
        [HttpGet("track-my-apps/{userId}")]
        [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
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