namespace HireUp.DTOs.Disabled;

public record DisabledRegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Gender,
    DateOnly Birthday,
    string PhoneNumber,
    int LocationId
);