namespace Application.Common;

public class PaginatedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int Page { get; set; }
    public int Size { get; set; }
    public int TotalItems { get; set; }
    
    public PaginatedResponse(IEnumerable<T> items, int page, int size, int totalItems)
    {
        Items = items;
        Page = page;
        Size = size;
        TotalItems = totalItems;
    }
}