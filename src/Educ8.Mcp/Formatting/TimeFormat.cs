using System.Globalization;

namespace Educ8.Mcp.Formatting;

/// <summary>Shared local-time formatting for model-facing text (invariant culture, 24-hour clock).</summary>
internal static class TimeFormat
{
    public static DateTimeOffset Local(DateTimeOffset value, TimeZoneInfo zone) => TimeZoneInfo.ConvertTime(value, zone);

    /// <summary>"Thu 2026-11-19 12:00–14:00", or a date span when it crosses days.</summary>
    public static string When(DateTimeOffset? start, DateTimeOffset? stop, TimeZoneInfo zone)
    {
        if (start is null) return "undated";
        var s = Local(start.Value, zone);
        if (stop is null) return s.ToString("ddd yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        var e = Local(stop.Value, zone);
        return s.Date == e.Date
            ? $"{s.ToString("ddd yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}–{e:HH:mm}"
            : $"{s:yyyy-MM-dd} – {e:yyyy-MM-dd}";
    }

    /// <summary>"12:00–14:00" within a day; with dates when the event spans days.</summary>
    public static string TimeSpan(DateTimeOffset start, DateTimeOffset stop, TimeZoneInfo zone)
    {
        var s = Local(start, zone);
        var e = Local(stop, zone);
        if (s.Date == e.Date) return s.TimeOfDay == System.TimeSpan.Zero && e.TimeOfDay == System.TimeSpan.Zero ? "all day" : $"{s:HH:mm}–{e:HH:mm}";
        return $"{s:yyyy-MM-dd HH:mm} – {e:yyyy-MM-dd HH:mm}";
    }

    /// <summary>"Thu 2026-11-19".</summary>
    public static string Day(DateTime localDate) => localDate.ToString("ddd yyyy-MM-dd", CultureInfo.InvariantCulture);
}
