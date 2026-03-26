namespace HireUp.Database.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ISkillRepository Skills { get; }
        IJobListingRepository JobListings { get; }
        IMockInterviewRepository MockInterviews { get; }
        IApplicationRepository Applications { get; }
        ILocationRepository Locations { get; }
        public IReviewRepository Reviews { get; }
        public IFollowsRepository Follows { get;}
        Task<int> SaveChangesAsync();
    }
}