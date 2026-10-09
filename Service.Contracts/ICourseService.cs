using LMS.Shared.DTOs.CourseDtos;

namespace Service.Contracts;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetCoursesAsync();
    Task<IEnumerable<CourseDto>> GetCourseAsync(int id);
    Task<CourseDto> NewCourseAsync(NewCourseDto course);
}
