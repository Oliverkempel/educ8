namespace Educ8.Itslearning.Models;

/// <summary>The signed-in user (<c>GET restapi/personal/person/v1</c>).</summary>
public sealed record Person
{
    public int PersonId { get; init; }
    public string FirstName { get; init; } = "";
    public string LastName { get; init; } = "";
    public string? FullName { get; init; }

    /// <summary>Language code like "en-GB" or "da-DK".</summary>
    public string? Language { get; init; }

    public string? TimeZoneId { get; init; }

    public string DisplayName => string.IsNullOrWhiteSpace(FullName) ? $"{FirstName} {LastName}".Trim() : FullName!;
}
