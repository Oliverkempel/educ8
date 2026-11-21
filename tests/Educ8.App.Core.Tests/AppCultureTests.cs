using System.Globalization;
using Educ8.App.Core.Presentation;

namespace Educ8.App.Core.Tests;

public class AppCultureTests
{
    private static readonly DateTime Afternoon = new(2026, 11, 16, 14, 5, 0);
    private static readonly CultureInfo Us = CultureInfo.GetCultureInfo("en-US");

    /// <summary>ICU puts a narrow no-break space before AM/PM.</summary>
    private static string N(string s) => s.Replace('\u202F', ' ');

    private static Func<string, string?> Env(params (string Key, string Value)[] vars) =>
        key => vars.FirstOrDefault(v => v.Key == key).Value;

    [Fact]
    public void Desktop_24_hour_clock_wins_over_the_locales_12_hour_one()
    {
        var c = AppCulture.Create(Us, Env(), use24Hour: true);
        Assert.Equal("14:05", Afternoon.ToString("t", c));
        Assert.Equal("08:30", new DateTime(2026, 11, 16, 8, 30, 0).ToString("t", c));
        Assert.Equal("14:05:00", Afternoon.ToString("T", c));
        Assert.Equal("2:05 PM", N(Afternoon.ToString("t", Us))); // the original culture is untouched
    }

    [Fact]
    public void Desktop_12_hour_clock_on_a_24_hour_locale()
    {
        var c = AppCulture.Create(CultureInfo.GetCultureInfo("en-GB"), Env(), use24Hour: false);
        Assert.Equal("2:05 PM", N(Afternoon.ToString("t", c)).ToUpperInvariant());
    }

    [Fact]
    public void No_preference_keeps_the_locales_clock()
    {
        Assert.Equal("2:05 PM", N(Afternoon.ToString("t", AppCulture.Create(Us, Env(), use24Hour: null))));
    }

    [Fact]
    public void Formats_follow_lc_time_and_lc_numeric_with_lc_all_overriding()
    {
        var c = AppCulture.Create(Us, Env(("LC_TIME", "da_DK.UTF-8"), ("LC_NUMERIC", "de_DE.UTF-8")), use24Hour: null);
        Assert.Equal("14.05", Afternoon.ToString("t", c));
        Assert.Equal("1,5", 1.5.ToString(c));
        Assert.Equal("en-US", c.Name); // language stays

        var all = AppCulture.Create(Us, Env(("LC_ALL", "en_GB.UTF-8"), ("LC_TIME", "da_DK.UTF-8")), use24Hour: null);
        Assert.Equal("14:05", Afternoon.ToString("t", all));

        var posix = AppCulture.Create(Us, Env(("LC_TIME", "C"), ("LC_NUMERIC", "not_a_locale")), use24Hour: null);
        Assert.Equal("2:05 PM", N(Afternoon.ToString("t", posix)));
    }

    [Fact]
    public void Day_follows_the_cultures_order()
    {
        var before = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = Us;
            Assert.Equal("Mon Nov 16", Texts.Day(Afternoon, weekday: true, year: false));
            Assert.Equal(("November 16", "November 16, 2026", false), (Texts.MonthDay(Afternoon), Texts.FullDate(Afternoon), Texts.DayFirst));
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("da-DK");
            Assert.Equal(("16. november", "16. november 2026", true), (Texts.MonthDay(Afternoon), Texts.FullDate(Afternoon), Texts.DayFirst));
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-GB");
            Assert.Equal("Mon 16 Nov 2026", Texts.Day(Afternoon, weekday: true, year: true));
        }
        finally
        {
            CultureInfo.CurrentCulture = before;
        }
    }

    [Fact]
    public void Span_over_several_days_names_both_days()
    {
        var before = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-GB");
            var now = new DateTimeOffset(2026, 11, 18, 12, 0, 0, TimeSpan.Zero);
            Assert.Equal("Mon 16 Nov – Fri 4 Dec", Texts.Span(new DateTimeOffset(2026, 11, 16, 0, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2026, 12, 5, 0, 0, 0, TimeSpan.Zero), now, TimeZoneInfo.Utc));
            Assert.Equal("Mon 16 Nov, 08:00 – Fri 4 Dec, 23:55", Texts.Span(new DateTimeOffset(2026, 11, 16, 8, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2026, 12, 4, 23, 55, 0, TimeSpan.Zero), now, TimeZoneInfo.Utc));
            Assert.Equal("Tue 17 Nov, 13:00–15:00", Texts.Span(new DateTimeOffset(2026, 11, 17, 13, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2026, 11, 17, 15, 0, 0, TimeSpan.Zero), now, TimeZoneInfo.Utc));
        }
        finally
        {
            CultureInfo.CurrentCulture = before;
        }
    }
}
