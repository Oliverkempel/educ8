using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Shell;

namespace Educ8.App.Gtk;

/// <summary>
/// Development aid (EDUC8_SCREENSHOT_DIR): walks the main screens with live data, renders the window to PNG after each step,
/// then quits. Lets the UI be checked without a visible window (e.g. under GDK_BACKEND=broadway).
/// </summary>
internal static class Screenshots
{
    public static async Task RunAsync(Adw.Application app, MainWindow main, ShellViewModel shell, string dir)
    {
        Directory.CreateDirectory(dir);
        try
        {
            await Until(() => shell.State != ShellState.Starting);
            await Task.Delay(500);
            if (shell.State == ShellState.SignedOut)
            {
                await Save(main.Window, Path.Combine(dir, "welcome.png"));
                return;
            }

            await Until(() => !shell.Courses.IsLoading && (shell.Courses.Courses.Count > 0 || shell.Courses.EmptyText is not null || shell.Courses.ErrorMessage is not null));
            await Task.Delay(500);
            await Save(main.Window, Path.Combine(dir, "courses.png"));

            // The list layout with the first rows expanded (current plans + teachers), then the first current plan's page.
            var layout = shell.Courses.Layout;
            shell.Courses.Layout = Core.Services.CoursesLayout.Grid;
            await Task.Delay(500);
            await Save(main.Window, Path.Combine(dir, "courses-grid.png"));
            shell.Courses.Layout = Core.Services.CoursesLayout.List;
            var expanded = shell.Courses.Courses.Take(3).ToList();
            foreach (var card in expanded) card.IsExpanded = true;
            await Until(() => expanded.All(c => !c.ActivePlans.IsLoading));
            await Task.Delay(800);
            await Save(main.Window, Path.Combine(dir, "courses-list.png"));
            if (expanded.FirstOrDefault(c => c.ActivePlans.Items.Count > 0) is { } withPlan)
            {
                withPlan.OpenPlanCommand.Execute(withPlan.ActivePlans.Items[0]);
                await Task.Delay(800);
                await Save(main.Window, Path.Combine(dir, "plan.png"));
                main.Courses.Navigation.Pop();
                await Task.Delay(500);
            }
            foreach (var card in expanded) card.IsExpanded = false;
            shell.Courses.Layout = layout; // the user's saved choice stays as it was

            // A course page: overview, then the Resources tab, then back to the grid.
            if (shell.Courses.Courses.Count > 0)
            {
                CourseViewModel? course = null;
                void Opened(object? sender, CourseViewModel c) => course = c;
                shell.Courses.CourseOpened += Opened;
                shell.Courses.OpenCourseCommand.Execute(shell.Courses.Courses[0]);
                shell.Courses.CourseOpened -= Opened;
                await Until(() => course is { IsLoading: false });
                await Task.Delay(800);
                await Save(main.Window, Path.Combine(dir, "course.png"));
                First<Adw.ViewStack>(main.Window)?.SetVisibleChildName("plans");
                await Until(() => !course!.AllPlans.IsLoading && (course.AllPlans.Plans.Count > 0 || course.AllPlans.EmptyText is not null || course.AllPlans.ErrorMessage is not null));
                await Task.Delay(800);
                await Save(main.Window, Path.Combine(dir, "course-plans.png"));
                First<Adw.ViewStack>(main.Window)?.SetVisibleChildName("resources");
                await Until(() => !course!.Resources.IsLoading && (course.Resources.Items.Count > 0 || course.Resources.EmptyText is not null || course.Resources.ErrorMessage is not null));
                await Task.Delay(800);
                await Save(main.Window, Path.Combine(dir, "course-resources.png"));
                main.Courses.Navigation.Pop();
                await Task.Delay(500);
            }

            // Notifications: listing only (opening one would mark it read on itslearning).
            shell.CurrentPage = AppPage.Notifications;
            await Until(() => !shell.Notifications.IsLoading);
            await Task.Delay(500);
            await Save(main.Window, Path.Combine(dir, "notifications.png"));
            // An already-read notification about a course: opens the course page in the app and writes nothing.
            if (shell.Notifications.Items.FirstOrDefault(n => n.IsRead && n.OpensInApp) is { } announcement)
            {
                shell.Notifications.OpenCommand.Execute(announcement);
                await Task.Delay(2500);
                await Save(main.Window, Path.Combine(dir, "notification-course.png"));
            }

            // Messages: the list, a conversation that's already read (opening it writes nothing), and an empty new-conversation form.
            shell.CurrentPage = AppPage.Messages;
            await Until(() => !shell.Messages.IsLoading);
            await Task.Delay(500);
            await Save(main.Window, Path.Combine(dir, "messages.png"));
            var read = shell.Messages.Threads.Where(t => !t.IsUnread).ToList();
            if ((read.FirstOrDefault(t => t.CanReply) ?? read.FirstOrDefault()) is { } thread)
            {
                shell.Messages.OpenCommand.Execute(thread);
                await Task.Delay(2500);
                await Save(main.Window, Path.Combine(dir, "conversation.png"));
                main.Messages.Navigation.Pop();
            }
            shell.Messages.NewConversationCommand.Execute(null);
            await Task.Delay(800);
            await Save(main.Window, Path.Combine(dir, "message-new.png"));
            main.Messages.Navigation.Pop();
            await Task.Delay(300);

            shell.CurrentPage = AppPage.Mcp;
            await Task.Delay(1000);
            await Save(main.Window, Path.Combine(dir, "mcp.png"));

            shell.CurrentPage = AppPage.Calendar;
            await Until(() => !shell.Calendar.IsLoading && shell.Calendar.Days.Count == 7);
            await Task.Delay(500);
            await Save(main.Window, Path.Combine(dir, "calendar.png"));

            // Pick a course in the filter through the drop-down itself (the path that used to crash), then back to all.
            if (shell.Calendar.CourseFilters.Count > 1 && First<global::Gtk.DropDown>(main.Window) is { } filter)
            {
                filter.SetSelected(1);
                await Task.Delay(800);
                await Save(main.Window, Path.Combine(dir, "calendar-filter.png"));
                filter.SetSelected(0);
                await Task.Delay(300);
            }

            var first = shell.Calendar.Days.SelectMany(d => d.Events).FirstOrDefault(e => e.Start >= DateTimeOffset.Now.Date)
                        ?? shell.Calendar.Days.SelectMany(d => d.Events).FirstOrDefault();
            if (first is not null)
            {
                shell.Calendar.SelectedEvent = first;
                await Until(() => shell.Calendar.Details.HasEvent && !shell.Calendar.Details.IsLoading);
                await Task.Delay(500);
                await Save(main.Window, Path.Combine(dir, "event.png"));
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Screenshots failed: {e}");
        }
        finally
        {
            app.Quit();
        }
    }

    private static T? First<T>(global::Gtk.Widget root) where T : global::Gtk.Widget
    {
        for (var child = root.GetFirstChild(); child is not null; child = child.GetNextSibling())
        {
            if (child is T match && child.GetMapped()) return match;
            if (First<T>(child) is { } found) return found;
        }
        return null;
    }

    private static async Task Until(Func<bool> condition, int timeoutSeconds = 30)
    {
        var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
        while (!condition())
        {
            if (DateTime.UtcNow > deadline) throw new TimeoutException("Timed out waiting for the UI.");
            await Task.Delay(200);
        }
    }

    private static async Task Save(global::Gtk.Window window, string path)
    {
        // Broadway only draws while a viewer is connected and visible; wait for a drawn frame.
        global::Gsk.RenderNode? node = null;
        for (var attempt = 0; attempt < 50 && node is null; attempt++)
        {
            if (attempt > 0) await Task.Delay(200);
            var paintable = global::Gtk.WidgetPaintable.New(window);
            var snapshot = global::Gtk.Snapshot.New();
            paintable.Snapshot(snapshot, window.GetWidth(), window.GetHeight());
            node = snapshot.ToNode();
        }
        if (node is null) throw new InvalidOperationException("Nothing to render. Is a Broadway viewer connected and visible?");
        var texture = window.GetNative()!.GetRenderer()!.RenderTexture(node, null);
        texture.SaveToPng(path);
        Console.Error.WriteLine($"saved {path}");
    }
}
