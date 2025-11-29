namespace HireUp.Database.Interfaces
{
    public interface IMockInterviewRepository : IRepository<MockInterview>
    {
        Task<IEnumerable<MockInterview>> GetScheduledInterviewsAsync();
        Task<IEnumerable<MockInterview>> GetInterviewsByUserAsync(string userId);
        Task<IEnumerable<MockInterview>> GetCompletedInterviewsWithFeedbackAsync();
        Task<bool> IsTimeSlotAvailableAsync(string interviewerId, DateTime scheduledAt, int duration);
    }
}