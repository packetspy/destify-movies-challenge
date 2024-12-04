namespace MoviesChallenge.Domain.Models;
public class PagedResult<T>
{
    public IEnumerable<T>? Data { get; set; }

    public PagedMetadata? Meta { get; set; } = new PagedMetadata
    {
        Page = 1,
        PageSize = 10,
        TotalCount = 0,
        TotalPages = 0
    };
}