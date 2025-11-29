namespace HireUp.Database.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsersWithDisabilitiesAsync();
        //Task<IEnumerable<ApplicationUser>> GetAvailableInterviewersAsync();
    }
}