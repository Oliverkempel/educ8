using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Presentation;

/// <summary>
/// The user's courses, loaded once and shared by the screens that need them (courses page, calendar colours and filter).
/// </summary>
public sealed class CourseCatalog(Func<ItslearningClient> client)
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private IReadOnlyList<PersonCourse>? _courses;

    /// <summary>Courses, loading them the first time (or again when <paramref name="refresh"/> is set).</summary>
    public async Task<IReadOnlyList<PersonCourse>> GetAsync(bool refresh = false, CancellationToken ct = default)
    {
        if (!refresh && _courses is { } cached) return cached;
        await _gate.WaitAsync(ct);
        try
        {
            if (!refresh && _courses is { } again) return again;
            var list = new List<PersonCourse>();
            await foreach (var c in client().GetAllCoursesAsync(ct)) list.Add(c);
            _courses = list;
            return list;
        }
        finally
        {
            _gate.Release();
        }
    }

    /// <summary>Forget the cached courses (e.g. after sign-out).</summary>
    public void Clear() => _courses = null;

    /// <summary>The cached course, if loaded.</summary>
    public PersonCourse? Find(int courseId) => _courses?.FirstOrDefault(c => c.CourseId == courseId);
}
