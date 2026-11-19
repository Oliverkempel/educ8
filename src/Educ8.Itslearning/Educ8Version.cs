using System.Reflection;

namespace Educ8.Itslearning;

/// <summary>
/// The release version (date-based, e.g. <c>2026.11.19</c> or <c>2026.11.19.1</c>; <c>0.0.0-dev</c> for local builds).
/// CI sets it with <c>-p:Version=…</c>; the SDK appends <c>+&lt;commit&gt;</c> to the informational version.
/// </summary>
public static class Educ8Version
{
    /// <summary>The version without the commit, e.g. "2026.11.19.1".</summary>
    public static string Of(Assembly assembly) => Full(assembly).Split('+')[0];

    /// <summary>The version with the commit, e.g. "2026.11.19.1+3f2a1bc…".</summary>
    public static string Full(Assembly assembly) =>
        assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? assembly.GetName().Version?.ToString() ?? "0.0.0";
}
