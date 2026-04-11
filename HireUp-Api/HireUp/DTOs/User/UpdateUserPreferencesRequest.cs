namespace HireUp.DTOs.User
{
    public class UpdateUserPreferencesRequest
    {
        public List<int> JobTypeIds { get; set; }
        public List<int> OfficeTypeIds { get; set; }
        public List<int> LocationIds { get; set; }
        public List<int> JobCategoryIds { get; set; }
        public List<int> JobRoleIds { get; set; }
    }
}
