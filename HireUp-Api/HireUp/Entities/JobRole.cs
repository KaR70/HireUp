
namespace HireUp.Entities
{
    public class JobRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}


