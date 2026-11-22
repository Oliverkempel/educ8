using Gtk;

namespace Educ8.App.Gtk.Ui;

/// <summary>
/// A thin progress line along the bottom edge of a row (a stock <see cref="ProgressBar"/> with the <c>osd</c> style), for
/// downloads. Shows the fraction when it's known and pulses when it isn't; hidden when idle.
/// </summary>
internal sealed class RowProgress
{
    private readonly ProgressBar _bar;
    private uint _pulse;

    private RowProgress(ProgressBar bar) => _bar = bar;

    /// <summary>
    /// Adds the progress line as a second child of the row: list rows lay their children out as layers (bin layout), so it spans
    /// the row's full width at the bottom while the row's own content (and its stock styling) stays untouched.
    /// </summary>
    public static RowProgress Attach(ListBoxRow row)
    {
        var bar = ProgressBar.New();
        bar.AddCssClass("osd");
        bar.SetValign(Align.End);
        bar.SetCanTarget(false);
        bar.SetVisible(false);
        bar.SetParent(row);
        var progress = new RowProgress(bar);
        row.OnDestroy += (_, _) =>
        {
            progress.StopPulse();
            bar.Unparent();
        };
        return progress;
    }

    /// <param name="busy">Whether a download is running.</param>
    /// <param name="fraction">How far it got (0–1), or null when unknown.</param>
    public void Show(bool busy, double? fraction)
    {
        _bar.SetVisible(busy);
        if (busy && fraction is { } f)
        {
            StopPulse();
            _bar.SetFraction(f);
        }
        else if (busy && _pulse == 0)
        {
            _bar.Pulse();
            _pulse = GLib.Functions.TimeoutAdd(GLib.Constants.PRIORITY_DEFAULT, 120, () =>
            {
                _bar.Pulse();
                return true;
            });
        }
        else if (!busy)
        {
            StopPulse();
            _bar.SetFraction(0);
        }
    }

    private void StopPulse()
    {
        if (_pulse == 0) return;
        GLib.Functions.SourceRemove(_pulse);
        _pulse = 0;
    }
}
