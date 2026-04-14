namespace HireUp.DTOs.User
{
    public class UpdateUserPreferencesRequest
    {
        public List<int> JobTypeIds { get; set; }
        public List<int> OfficeTypeIds { get; set; }
        public List<int> LocationIds { get; set; }
        //TODO: Decide wether to keep this or not
        // public List<int> JobCategoryIds { get; set; }
        public int? JobRoleId { get; set; }
    }
}
