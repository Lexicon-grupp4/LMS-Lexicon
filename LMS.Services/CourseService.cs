using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.CourseDtos;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;

namespace LMS.Services;

public class CourseService : ICourseService
{
    private ApplicationDbContext _context;

    public CourseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
    {
        return await _context.Courses.Select(c => new CourseDto(
            c.Id,
            c.Name,
            c.Code,
            c.Description,
            c.StartDate,
            c.EndDate
            )).ToListAsync();
    }

    public async Task<IEnumerable<CourseDto>> GetCourseAsync(int id)
    {
        return await _context.Courses.Where(c => c.Id == id).Select(c => new CourseDto(
            c.Id,
            c.Name,
            c.Code,
            c.Description,
            c.StartDate,
            c.EndDate
            )).ToListAsync();
    }

    public async Task<CourseDto> NewCourseAsync(NewCourseDto course)
    {
        var c = new Domain.Models.Entities.Course();
        c.Name = course.Name;
        c.Code = course.Code;
        c.Description = course.Description;
        c.StartDate = course.StartDate;
        c.EndDate = course.EndDate;
        await _context.Courses.AddAsync(c);
        await _context.SaveChangesAsync();
        return new CourseDto(c.Id, c.Name, c.Code, c.Description, c.StartDate, c.EndDate);
    }
}
