namespace Application.Common.Mappings;

public record CustomerDto 
{
    public CustomerDto() { }

    public string Name { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
