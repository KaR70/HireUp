using System.Collections.Generic;

namespace HireUp.Entities
{
    public class JobType
    {
        public int Id { get; set; }
        public string Name { get; set; }

       
        public const string FullTime = "Full-time";
        public const string Contract = "Contract";
        public const string Remote = "Remote";
        public const string PartTime = "Part-time";

        public virtual ICollection<UserJobTypePreference> UserJobTypePreferences { get; set; } = new HashSet<UserJobTypePreference>();
    }
}