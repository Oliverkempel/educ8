namespace Educ8.Itslearning.Models;

/// <summary>itslearning's paged list wrapper. Page size is capped at 100 by the API.</summary>
public sealed record EntityList<T>
{
    public IReadOnlyList<T> EntityArray { get; init; } = [];
    public int Total { get; init; }
    public int CurrentPageIndex { get; init; }
    public int PageSize { get; init; }
}
