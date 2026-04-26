using Microsoft.EntityFrameworkCore.Storage;

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
        ICompanyRepository Companies { get; }
        public IReviewRepository Reviews { get; }
        public IFollowsRepository Follows { get;}
        public IRepository<Industry> Industry { get; }
        IExperienceLevelRepository ExperienceLevel { get; }
        
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        IExecutionStrategy CreateExecutionStrategy();
    }
}