using HireUp.Database.Interfaces;

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
        }

        public IUserRepository Users { get; }
        public ISkillRepository Skills { get; }
        public IJobListingRepository JobListings { get; }
        public IMockInterviewRepository MockInterviews { get; }
        public IApplicationRepository Applications { get; }
        public ILocationRepository Locations { get; }
        public IReviewRepository Reviews { get; set; }
        public IFollowsRepository Follows { get; set; }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context?.Dispose();
    }
}