namespace Bank.Api.Contracts.Pagination;

public abstract record PaginationResponse<T>
{
    protected PaginationResponse(IReadOnlyCollection<T> items, int totalCount, int pageSize)
    {
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        TotalCount = totalCount;
        Items = items;
    }

    public int TotalPages { get; }
    public int TotalCount { get; }
    public IReadOnlyCollection<T> Items { get; }
}
