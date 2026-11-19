namespace Educ8.Itslearning.Models;

/// <summary>What a course element really is. Derived from ElementType + LearningToolId (see docs/api/live-findings.md).</summary>
public enum ResourceKind
{
    Other,
    Folder,
    Page,
    File,
    Link,
    Assignment,
    Test,
    Survey,
    Activity,
    LearningPath,
}

/// <summary>Known itslearning learning tool ids (observed at SDU, 2026-11-18).</summary>
public static class LearningTools
{
    public const int Page = 5;
    public const int Test = 5002;
    public const int Assignment = 5006;
    public const int File = 5009;
    public const int Link = 5010;
    public const int Survey = 5050;

    public static ResourceKind KindOf(string? elementType, int learningToolId) => elementType switch
    {
        "Folder" => ResourceKind.Folder,
        "LearningPath" => ResourceKind.LearningPath,
        "CustomActivity" => ResourceKind.Activity,
        "Assignment" or "Essay" => ResourceKind.Assignment,
        "FolderFile" => ResourceKind.File,
        "WebLink" => ResourceKind.Link,
        "Test" or "NttTest" => ResourceKind.Test,
        "Survey" => ResourceKind.Survey,
        "LearningToolElement" => learningToolId switch
        {
            Page => ResourceKind.Page,
            File => ResourceKind.File,
            Link => ResourceKind.Link,
            Assignment => ResourceKind.Assignment,
            Test => ResourceKind.Test,
            Survey => ResourceKind.Survey,
            _ => ResourceKind.Other,
        },
        _ => ResourceKind.Other,
    };
}
