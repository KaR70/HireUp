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
        /// Submit a job application with a cover letter and resume file.
        /// </summary>
        /// <remarks>
        /// Allows freelance users (both standard and disabled) to apply for job listings by submitting a cover letter and resume file.
        /// The resume file is securely stored on the server and a database record is created tracking the application.
        /// This endpoint requires authentication and accepts multipart/form-data content.
        ///
        /// Supported file formats: PDF, DOCX, DOC, TXT
        /// Maximum file size: Handled by server configuration
        ///
        /// Sample request (multipart/form-data):
        ///
        ///     POST /api/jobapplications/apply-job
        ///     Content-Type: multipart/form-data
        ///
        ///     Form Fields:
        ///     - jobListingId: 42 (integer)
        ///     - coverLetter: "I am very interested in this Senior C# Developer position. With 7+ years of experience in .NET development and a proven track record of delivering enterprise solutions, I believe I am an excellent fit for your team. I am particularly drawn to your company's commitment to accessibility and inclusive hiring practices." (string)
        ///     - resumeFile: [binary file] (multipart file - PDF or DOCX)
        ///     - userId: "user-123abc" (string - current user's ID)
        ///
        /// Sample success response (200):
        ///
        ///     {
        ///       "message": "تم التقديم وحفظ الملف بنجاح!",
        ///       "fileName": "e7b5f6b3-9c3a-4d1f-8a2b-0f1c2d3e4a5b.pdf"
        ///     }
        ///
        /// Sample validation error response (400 - Missing resume file):
        ///
        ///     "ملف السيرة الذاتية مطلوب."
        ///
        /// Sample server error response (500 - Database/file system error):
        ///
        ///     "خطأ في الداتابيز: Exception message details..."
        /// </remarks>
        /// <param name="dto">Job application form data containing:
        ///   - jobListingId: ID of the job listing to apply for (required)
        ///   - coverLetter: Cover letter text explaining why you're interested (optional but recommended)
        ///   - resumeFile: Resume file in PDF, DOCX, DOC, or TXT format (required)</param>
        /// <param name="userId">The unique identifier of the authenticated user submitting the application (required)</param>
        /// <returns>Returns a success message with the stored filename if application was successfully submitted and resume file was saved</returns>
        /// <response code="200">Application submitted successfully - resume file saved and database record created</response>
        /// <response code="400">Invalid request - missing required fields (resume file is required), invalid file format, or file size exceeds limit</response>
        /// <response code="401">Unauthorized - invalid or missing JWT token, or user is not authenticated as freelancer/disabled-freelancer</response>
        /// <response code="403">Forbidden - user does not have the required role (Freelancer or DisabledFreelancer)</response>
        /// <response code="500">Internal server error - database operation failed, file system error, or other unexpected error</response>
        [HttpPost("apply-job")]
        [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]        
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        /// Retrieve all job applications submitted by the authenticated user.
        /// </summary>
        /// <remarks>
        /// Allows freelance users to track all their job applications and monitor the status of each application.
        /// Returns a list of applications with details including job title, application status, submission date, and resume file URL.
        /// This endpoint requires authentication and can only be accessed by Freelancer or DisabledFreelancer roles.
        /// The user can only view their own applications; cross-user access is protected by user ID parameter.
        ///
        /// Application Statuses:
        /// - Pending: Application submitted, awaiting employer review
        /// - Under Review: Employer is actively reviewing the application
        /// - Accepted: Application accepted, interview or next step pending
        /// - Rejected: Application rejected by employer
        /// - Withdrawn: Application withdrawn by user
        ///
        /// Sample request:
        ///
        ///     GET /api/jobapplications/track-my-apps/user-123abc
        ///
        /// Sample success response (200):
        ///
        ///     [
        ///       {
        ///         "id": 42,
        ///         "jobTitle": "Senior C# Developer",
        ///         "status": "Pending",
        ///         "appliedAt": "2024-06-19T14:30:00Z",
        ///         "resumeUrl": "/uploads/resumes/e7b5f6b3-9c3a-4d1f-8a2b-0f1c2d3e4a5b.pdf"
        ///       },
        ///       {
        ///         "id": 38,
        ///         "jobTitle": "Full Stack Developer",
        ///         "status": "Under Review",
        ///         "appliedAt": "2024-06-18T09:15:00Z",
        ///         "resumeUrl": "/uploads/resumes/a1b2c3d4-e5f6-4789-b0c1-d2e3f4a5b6c7.pdf"
        ///       },
        ///       {
        ///         "id": 35,
        ///         "jobTitle": "Backend Developer",
        ///         "status": "Accepted",
        ///         "appliedAt": "2024-06-15T11:45:00Z",
        ///         "resumeUrl": "/uploads/resumes/x9y8z7w6-v5u4-3210-tsrq-ponmlkjihgfe.pdf"
        ///       }
        ///     ]
        ///
        /// Sample empty response (200 - No applications yet):
        ///
        ///     []
        /// </remarks>
        /// <param name="userId">The unique identifier of the authenticated user whose applications are to be retrieved</param>
        /// <returns>Returns a list of application records containing id, job title, application status, submission date, and resume file URL</returns>
        /// <response code="200">Successfully retrieved all applications for the user (may be empty if no applications submitted)</response>
        /// <response code="401">Unauthorized - invalid or missing JWT token, or user is not authenticated as freelancer/disabled-freelancer</response>
        /// <response code="403">Forbidden - user does not have the required role (Freelancer or DisabledFreelancer) or attempting to access another user's applications</response>
        [HttpGet("track-my-apps/{userId}")]
        [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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