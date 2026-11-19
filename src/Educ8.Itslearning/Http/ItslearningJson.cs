using System.Text.Json;

namespace Educ8.Itslearning.Http;

/// <summary>JSON settings for itslearning responses (PascalCase properties, tolerant of unknown fields).</summary>
public static class ItslearningJson
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
    };

    /// <summary>For request bodies: PascalCase property names, as in itslearning's docs.</summary>
    public static readonly JsonSerializerOptions Request = new() { PropertyNamingPolicy = null };
}
