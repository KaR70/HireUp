namespace HireUp.Entities
{
    public class UserOfficeTypePreference
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int OfficeTypeId { get; set; }
        public OfficeType OfficeType { get; set; }
    }
}
