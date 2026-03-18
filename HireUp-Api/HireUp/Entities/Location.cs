
namespace HireUp.Entities
{
    public class Location

    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserJobTypePreference> UserPreferences { get; set; } = new List<UserJobTypePreference>();
    }
}

