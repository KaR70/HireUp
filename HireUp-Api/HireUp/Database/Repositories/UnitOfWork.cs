using HireUp.Database.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace HireUp.Database.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context, IUserRepository users)
        {
            _context = context;
            Users = users;

            Skills = new SkillRepository(_context);
            JobListings = new JobListingRepository(_context);
            MockInterviews = new MockInterviewRepository(_context);
            Applications = new ApplicationRepository(_context);
            Locations = new LocationRepository(_context);
            Reviews = new ReviewRepository(_context);
            Follows = new FollowsRepository(_context);
            Companies = new CompanyRepository(_context);
            Industry = new IndustryRepository(context);
            ExperienceLevel = new ExperienceLevelRepository(context);
            DisabilityTypes = new DisabilityTypesRepository(_context);
            AccessibilityNeeds = new AccessibilityNeedsRepository(_context);
        }

        public IUserRepository Users { get; }
        public ISkillRepository Skills { get; }
        public IJobListingRepository JobListings { get; }
        public IMockInterviewRepository MockInterviews { get; }
        public IApplicationRepository Applications { get; }
        public ILocationRepository Locations { get; }
        public ICompanyRepository Companies { get; }
        public IReviewRepository Reviews { get; set; }
        public IFollowsRepository Follows { get; set; }
        public IRepository<Industry> Industry { get; }
        public IExperienceLevelRepository ExperienceLevel { get; }
        public IDisabilityTypesRepository DisabilityTypes { get; }
        public IAccessibilityNeedsRepository AccessibilityNeeds { get; }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
            await _context.Database.BeginTransactionAsync(cancellationToken);
        public IExecutionStrategy CreateExecutionStrategy() => _context.Database.CreateExecutionStrategy();
        public void Dispose() => _context?.Dispose();
    }
}