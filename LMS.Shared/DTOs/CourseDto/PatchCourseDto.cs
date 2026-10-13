namespace LMS.Shared.DTOs.CourseDtos;

public sealed record PatchCourseDto(
        int Id,
        string? Name,
        string? Code,
        string? Description,
        DateTime? StartDate,
        DateTime? EndDate
    );
