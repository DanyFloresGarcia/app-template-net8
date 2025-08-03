namespace Application.Common;

public record PaginatedResponse<T>
{
    public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();
    public int Page { get; init; }
    public int Size { get; init; }
    public int TotalItems { get; init; }

    public PaginatedResponse(IEnumerable<T> items, int page, int size, int totalItems)
    {
        Items = items;
        Page = page;
        Size = size;
        TotalItems = totalItems;
    }
}