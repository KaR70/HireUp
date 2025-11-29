namespace HireUp.Database.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ISkillRepository Skills { get; }
        IJobListingRepository JobListings { get; }
        IMockInterviewRepository MockInterviews { get; }
        IApplicationRepository Applications { get; }
        Task<int> SaveChangesAsync();
    }
}