using Microsoft.EntityFrameworkCore;
using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories
{
    public class MockInterviewRepository : BaseRepository<MockInterview>, IMockInterviewRepository
    {
        public MockInterviewRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<MockInterview>> GetScheduledInterviewsAsync()
            => await _dbSet.Where(m => m.Status == InterviewStatus.Scheduled && m.ScheduledAt > DateTime.UtcNow)
                          .Include(m => m.JobSeeker)
                          .Include(m => m.Interviewer)
                          .ToListAsync();

        public async Task<IEnumerable<MockInterview>> GetInterviewsByUserAsync(string userId)
            => await _dbSet.Where(m => m.JobSeekerId == userId || m.InterviewerId == userId)
                          .Include(m => m.JobSeeker)
                          .Include(m => m.Interviewer)
                          .OrderByDescending(m => m.ScheduledAt)
                          .ToListAsync();

        public async Task<IEnumerable<MockInterview>> GetCompletedInterviewsWithFeedbackAsync()
            => await _dbSet.Where(m => m.Status == InterviewStatus.Completed && !string.IsNullOrEmpty(m.Feedback))
                          .Include(m => m.JobSeeker)
                          .Include(m => m.Interviewer)
                          .ToListAsync();

        public async Task<bool> IsTimeSlotAvailableAsync(string interviewerId, DateTime scheduledAt, int duration)
        {
            var endTime = scheduledAt.AddMinutes(duration);
            return !await _dbSet.AnyAsync(m => m.InterviewerId == interviewerId &&
                                              m.Status == InterviewStatus.Scheduled &&
                                              m.ScheduledAt < endTime &&
                                              m.ScheduledAt.AddMinutes(m.DurationMinutes) > scheduledAt);
        }
    }
}