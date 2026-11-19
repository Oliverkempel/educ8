using System.Globalization;
using System.Net;
using Educ8.App.Core.Resources;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Presentation;

/// <summary>Ready-to-show texts shared by all platform UIs. Uses the current culture for dates and times.</summary>
public static class Texts
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
            _ => string.Format(CultureInfo.CurrentCulture, Strings.UpdatedOn, day.ToString("d MMM yyyy", CultureInfo.CurrentCulture)),
        };
    }

    /// <summary>"12:00–14:00", "All day", or a span with dates.</summary>
    public static string TimeRange(DateTimeOffset start, DateTimeOffset end, TimeZoneInfo zone)
    {
        var s = TimeZoneInfo.ConvertTime(start, zone);
        var e = TimeZoneInfo.ConvertTime(end, zone);
        if (s.TimeOfDay == TimeSpan.Zero && e.TimeOfDay == TimeSpan.Zero && e > s) return Strings.AllDay;
        if (s.Date == e.Date) return $"{s.ToString("t", CultureInfo.CurrentCulture)}–{e.ToString("t", CultureInfo.CurrentCulture)}";
        return $"{s.ToString("g", CultureInfo.CurrentCulture)} – {e.ToString("g", CultureInfo.CurrentCulture)}";
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
}
