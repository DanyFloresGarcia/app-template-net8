namespace Application.Common.Mappings;

public record CustomerDto 
{
    public CustomerDto() { }

    public string Name { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
}
