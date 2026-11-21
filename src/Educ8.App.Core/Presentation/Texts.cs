using System.Globalization;
using System.Net;
using Educ8.App.Core.Resources;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Presentation;

/// <summary>Ready-to-show texts shared by all platform UIs. Uses the current culture for dates and times.</summary>
public static partial class Texts
{
    /// <summary>A friendly message for an exception from itslearning access.</summary>
    public static string Error(Exception e) => e switch
    {
        NotAuthenticatedException => Strings.ErrorSessionExpired,
        HttpRequestException => Strings.ErrorOffline,
        TaskCanceledException => Strings.ErrorOffline,
        ItslearningApiException api => string.Format(CultureInfo.CurrentCulture, Strings.ErrorServer, (int)api.StatusCode),
        ContentUnavailableException c => c.Message,
        _ => string.Format(CultureInfo.CurrentCulture, Strings.ErrorUnknown, e.Message),
    };

    /// <summary>"Updated today", "Updated yesterday", "Updated 3 days ago", "Updated 12 Nov 2026".</summary>
    public static string LastActivity(DateTimeOffset? value, DateTimeOffset now, TimeZoneInfo zone)
    {
        if (value is null) return Strings.NeverUpdated;
        var day = TimeZoneInfo.ConvertTime(value.Value, zone).Date;
        var today = TimeZoneInfo.ConvertTime(now, zone).Date;
        var days = (today - day).Days;
        return days switch
        {
            <= 0 => Strings.UpdatedToday,
            1 => Strings.UpdatedYesterday,
            < 7 => string.Format(CultureInfo.CurrentCulture, Strings.UpdatedDaysAgo, days),
            _ => string.Format(CultureInfo.CurrentCulture, Strings.UpdatedOn, Day(day, weekday: false, year: true)),
        };
    }

    /// <summary>"Today, 14:00", "Yesterday, 09:12", "Tomorrow, 10:00", "Mon 16 Nov, 10:00" (year added when not this year).</summary>
    public static string Date(DateTimeOffset value, DateTimeOffset now, TimeZoneInfo zone)
    {
        var local = TimeZoneInfo.ConvertTime(value, zone);
        var today = TimeZoneInfo.ConvertTime(now, zone).Date;
        var time = local.ToString("t", CultureInfo.CurrentCulture);
        var day = (local.Date - today).Days switch
        {
            0 => Strings.DayToday,
            -1 => Strings.DayYesterday,
            1 => Strings.DayTomorrow,
            _ => Day(local.DateTime, weekday: true, year: local.Year != today.Year),
        };
        return string.Format(CultureInfo.CurrentCulture, Strings.DateWithTime, day, time);
    }

    /// <summary>"16 November" (en-GB), "November 16" (en-US): the culture's month-day pattern.</summary>
    public static string MonthDay(DateTime day) =>
        day.ToString(CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern, CultureInfo.CurrentCulture);

    /// <summary>"16 November 2026" (en-GB), "November 16, 2026" (en-US): the culture's long date without the weekday.</summary>
    public static string FullDate(DateTime day)
    {
        var pattern = LeadingWeekday().Replace(CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern, "");
        return day.ToString(pattern.Length > 0 ? pattern : "d", CultureInfo.CurrentCulture);
    }

    /// <summary>True when the culture writes the day before the month ("16 November"), so a range can share the month.</summary>
    public static bool DayFirst => CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern.TrimStart().StartsWith('d');

    /// <summary>"dddd, " / "dddd " / "dddd 'den' " at the start of a long date pattern.</summary>
    [System.Text.RegularExpressions.GeneratedRegex(@"^\s*dddd[,.\s]*('[^']*'\s*)?")]
    private static partial System.Text.RegularExpressions.Regex LeadingWeekday();

    /// <summary>
    /// A short day in the current culture's order: "Mon 16 Nov" (en-GB), "Mon Nov 16" (en-US), "man. 16. nov." (da-DK). Built
    /// from the culture's month-day pattern with the month abbreviated; the year is appended when asked.
    /// </summary>
    public static string Day(DateTime day, bool weekday, bool year)
    {
        var format = CultureInfo.CurrentCulture.DateTimeFormat;
        var pattern = format.MonthDayPattern.Replace("MMMM", "MMM", StringComparison.Ordinal);
        if (weekday) pattern = "ddd " + pattern;
        if (year) pattern += " yyyy";
        return day.ToString(pattern, CultureInfo.CurrentCulture);
    }

    /// <summary>"Tue 17 Nov, 14:00–16:00", or just the day for all-day plans.</summary>
    public static string Span(DateTimeOffset? start, DateTimeOffset? end, DateTimeOffset now, TimeZoneInfo zone)
    {
        if (start is null) return "";
        if (end is null || end <= start) return Date(start.Value, now, zone);
        var local = TimeZoneInfo.ConvertTime(start.Value, zone);
        var today = TimeZoneInfo.ConvertTime(now, zone).Date;
        if (!IsSameDay(start.Value, end.Value, zone)) return DaysRange(start.Value, end.Value, zone, today.Year);
        var day = Day(local.DateTime, weekday: true, year: local.Year != today.Year);
        return string.Format(CultureInfo.CurrentCulture, Strings.DateWithTime, day, TimeRange(start.Value, end.Value, zone));
    }

    /// <summary>A friendly MCP client name: "Claude Desktop", "Claude Code", the client's own name, or "MCP client".</summary>
    public static string McpClient(string? name) => name?.ToLowerInvariant() switch
    {
        null or "" => Strings.McpClientUnknown,
        "claude-ai" or "claude desktop" => "Claude Desktop",
        "claude-code" => "Claude Code",
        _ => name,
    };

    /// <summary>"0.4 s", "12 s", "250 ms".</summary>
    public static string Duration(long milliseconds) => milliseconds switch
    {
        < 1000 => string.Format(CultureInfo.CurrentCulture, "{0} ms", milliseconds),
        < 10_000 => string.Format(CultureInfo.CurrentCulture, "{0:0.0} s", milliseconds / 1000.0),
        _ => string.Format(CultureInfo.CurrentCulture, "{0} s", milliseconds / 1000),
    };

    /// <summary>"12:00–14:00", "All day", or a span with dates.</summary>
    public static string TimeRange(DateTimeOffset start, DateTimeOffset end, TimeZoneInfo zone)
    {
        var s = TimeZoneInfo.ConvertTime(start, zone);
        var e = TimeZoneInfo.ConvertTime(end, zone);
        if (s.TimeOfDay == TimeSpan.Zero && e.TimeOfDay == TimeSpan.Zero && e > s) return Strings.AllDay;
        if (s.Date == e.Date) return $"{s.ToString("t", CultureInfo.CurrentCulture)}–{e.ToString("t", CultureInfo.CurrentCulture)}";
        return DaysRange(start, end, zone, s.Year);
    }

    /// <summary>True if the span starts and ends on the same local day (an end at midnight belongs to the day before).</summary>
    private static bool IsSameDay(DateTimeOffset start, DateTimeOffset end, TimeZoneInfo zone)
    {
        var s = TimeZoneInfo.ConvertTime(start, zone);
        var e = TimeZoneInfo.ConvertTime(end, zone);
        if (e > s && e.TimeOfDay == TimeSpan.Zero) e = e.AddTicks(-1);
        return s.Date == e.Date;
    }

    /// <summary>
    /// A span over several days: "Mon 16 Nov – Fri 4 Dec" for whole days, otherwise with times ("Mon 16 Nov, 08:00 – Fri 4 Dec, 23:55").
    /// Years are shown when not <paramref name="year"/>.
    /// </summary>
    private static string DaysRange(DateTimeOffset start, DateTimeOffset end, TimeZoneInfo zone, int year)
    {
        var s = TimeZoneInfo.ConvertTime(start, zone);
        var e = TimeZoneInfo.ConvertTime(end, zone);
        if (s.TimeOfDay == TimeSpan.Zero && e.TimeOfDay == TimeSpan.Zero)
            return $"{Day(s.DateTime, true, s.Year != year)} – {Day(e.AddDays(-1).DateTime, true, e.AddDays(-1).Year != year)}";
        string At(DateTimeOffset d) => string.Format(CultureInfo.CurrentCulture, Strings.DateWithTime,
            Day(d.DateTime, true, d.Year != year), d.ToString("t", CultureInfo.CurrentCulture));
        return $"{At(s)} – {At(e)}";
    }

    public static string Kind(ResourceKind kind) => kind switch
    {
        ResourceKind.Folder => Strings.KindFolder,
        ResourceKind.Page => Strings.KindPage,
        ResourceKind.File => Strings.KindFile,
        ResourceKind.Link => Strings.KindLink,
        ResourceKind.Assignment => Strings.KindAssignment,
        ResourceKind.Test => Strings.KindTest,
        ResourceKind.Survey => Strings.KindSurvey,
        ResourceKind.Activity => Strings.KindActivity,
        ResourceKind.LearningPath => Strings.KindLearningPath,
        _ => Strings.KindOther,
    };

    public static string Teachers(IReadOnlyList<string> names) => names.Count switch
    {
        0 => "",
        <= 3 => string.Format(CultureInfo.CurrentCulture, Strings.Teachers, string.Join(", ", names)),
        _ => string.Format(CultureInfo.CurrentCulture, Strings.Teachers,
            string.Format(CultureInfo.CurrentCulture, Strings.TeachersMore, string.Join(", ", names.Take(2)), names.Count - 2)),
    };

    /// <summary>The placeholder title for content the app doesn't show ("Video", "Table", …).</summary>
    public static string Unsupported(Documents.RichUnsupportedKind kind) => kind switch
    {
        Documents.RichUnsupportedKind.Video => Strings.ContentVideo,
        Documents.RichUnsupportedKind.Audio => Strings.ContentAudio,
        Documents.RichUnsupportedKind.Embed => Strings.ContentEmbed,
        Documents.RichUnsupportedKind.Interactive => Strings.ContentInteractive,
        Documents.RichUnsupportedKind.Table => Strings.ContentTable,
        _ => Strings.ContentOther,
    };

    /// <summary>The placeholder's button: "Watch" for videos with a link, "Open" for other links, else "Open in itslearning".</summary>
    public static string UnsupportedAction(Documents.RichUnsupportedKind kind, bool hasLink) =>
        !hasLink ? Strings.OpenInItslearning : kind is Documents.RichUnsupportedKind.Video ? Strings.ContentWatch : Strings.ContentOpenLink;
}
