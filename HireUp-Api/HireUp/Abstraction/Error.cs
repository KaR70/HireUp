namespace HireUp.Abstraction;

public class Error
{
    public string Code { get; set; }
    public string Description { get; set; }
    public int? StatusCode { get; }

    public Error(string code, string description, int? statusCode = null)
    {
        Code = code;
        Description = description;
        StatusCode = statusCode;
    }

    public static readonly Error None = new(string.Empty, string.Empty, null);
}
