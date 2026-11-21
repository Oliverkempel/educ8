namespace Educ8.App.Gtk.Services;

/// <summary>Desktop preferences that .NET doesn't see, read once at startup.</summary>
internal static class DesktopSettings
{
    private const string InterfaceSchema = "org.gnome.desktop.interface";
    private const string ClockFormatKey = "clock-format";

    /// <summary>
    /// The desktop's clock choice (GNOME Settings → Date &amp; Time → Time Format): true for 24-hour, false for AM/PM, null when
    /// unknown. Read through the settings portal (works inside Flatpak and snap), falling back to GSettings on the host.
    /// </summary>
    public static bool? Use24HourClock() => (FromPortal() ?? FromGSettings()) switch
    {
        "24h" => true,
        "12h" => false,
        _ => null,
    };

    private static string? FromPortal()
    {
        try
        {
            var bus = Gio.Functions.BusGetSync(Gio.BusType.Session, null);
            var reply = bus.CallSync("org.freedesktop.portal.Desktop", "/org/freedesktop/portal/desktop", "org.freedesktop.portal.Settings",
                "Read", GLib.Variant.NewTuple([GLib.Variant.NewString(InterfaceSchema), GLib.Variant.NewString(ClockFormatKey)]),
                null, Gio.DBusCallFlags.None, 2000, null);
            // Read returns (v); older portals wrap the value in one more variant.
            var value = reply.GetChildValue(0);
            while (value.GetTypeString() == "v") value = value.GetVariant();
            return value.GetTypeString() == "s" ? value.GetString(out _) : null;
        }
        catch (Exception e)
        {
            Launch.Debug($"settings portal: {e.GetType().Name}: {e.Message}");
            return null; // no portal (or no such key): fall back
        }
    }

    private static string? FromGSettings()
    {
        try
        {
            if (Gio.SettingsSchemaSource.GetDefault()?.Lookup(InterfaceSchema, true) is null) return null;
            return Gio.Settings.New(InterfaceSchema).GetString(ClockFormatKey);
        }
        catch (Exception e)
        {
            Launch.Debug($"gsettings: {e.GetType().Name}: {e.Message}");
            return null;
        }
    }
}
