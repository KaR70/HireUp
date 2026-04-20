namespace HireUp.Entities;

public class Industry
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Company> Companies { get; set; }
}