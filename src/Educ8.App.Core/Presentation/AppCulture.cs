using System.Globalization;
using System.Text.RegularExpressions;

namespace Educ8.App.Core.Presentation;

/// <summary>
/// The culture the app formats dates, times and numbers with: the system's. .NET takes the culture from <c>LANG</c>/<c>LC_ALL</c>
/// only, but on Linux the user can set formats separately (<c>LC_TIME</c>, <c>LC_NUMERIC</c>; GNOME's "Formats" sets them) and
/// choose a 12- or 24-hour clock regardless of the locale (GNOME's <c>clock-format</c>). On Windows and macOS .NET already
/// includes the user's regional overrides, so platforms there pass no clock preference.
/// </summary>
public static partial class AppCulture
{
    /// <summary>
    /// Builds the formatting culture: <paramref name="current"/> with date/time formats from <c>LC_TIME</c> and number formats
    /// from <c>LC_NUMERIC</c> (<c>LC_ALL</c> wins over both), then the clock forced to 24 or 12 hours if
    /// <paramref name="use24Hour"/> is set.
    /// </summary>
    public static CultureInfo Create(CultureInfo current, Func<string, string?> getEnv, bool? use24Hour)
    {
        var culture = (CultureInfo)current.Clone();
        var all = FromPosix(getEnv("LC_ALL"));
        if ((all ?? FromPosix(getEnv("LC_TIME"))) is { } time && time.Name != culture.Name)
            culture.DateTimeFormat = (DateTimeFormatInfo)time.DateTimeFormat.Clone();
        if ((all ?? FromPosix(getEnv("LC_NUMERIC"))) is { } numeric && numeric.Name != culture.Name)
            culture.NumberFormat = (NumberFormatInfo)numeric.NumberFormat.Clone();

        if (use24Hour is { } h24)
        {
            var f = culture.DateTimeFormat;
            f.ShortTimePattern = Clock(f.ShortTimePattern, h24);
            f.LongTimePattern = Clock(f.LongTimePattern, h24);
            if (!h24 && f.AMDesignator.Length == 0)
            {
                f.AMDesignator = "AM";
                f.PMDesignator = "PM";
            }
        }
        return culture;
    }

    /// <summary>Applies <paramref name="culture"/> to this thread and to threads started later (the UI thread and async continuations).</summary>
    public static void Apply(CultureInfo culture)
    {
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.CurrentCulture = culture;
    }

    /// <summary>Rewrites a time pattern for a 24-hour ("HH:mm") or 12-hour ("h:mm tt") clock, keeping its separators.</summary>
    internal static string Clock(string pattern, bool use24Hour)
    {
        if (use24Hour)
        {
            if (!pattern.Contains('h')) return pattern;
            return AmPm().Replace(pattern, "").Trim().Replace("hh", "HH").Replace("h", "HH").Replace("HHHH", "HH");
        }
        if (!pattern.Contains('H')) return pattern;
        return HoursOf24().Replace(pattern, "h") + " tt";
    }

    /// <summary>"da_DK.UTF-8@euro" → da-DK; null for C/POSIX, empty or unknown locales.</summary>
    internal static CultureInfo? FromPosix(string? locale)
    {
        if (string.IsNullOrWhiteSpace(locale)) return null;
        var name = locale.Split('.', '@')[0].Replace('_', '-');
        if (name is "C" or "POSIX" || name.Length == 0) return null;
        try
        {
            var culture = CultureInfo.GetCultureInfo(name);
            return culture.Name.Length == 0 ? null : culture;
        }
        catch (CultureNotFoundException)
        {
            return null;
        }
    }

    [GeneratedRegex(@"\s*t+\s*")]
    private static partial Regex AmPm();

    [GeneratedRegex("H+")]
    private static partial Regex HoursOf24();
}
