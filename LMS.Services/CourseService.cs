using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Migrations;
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

    public async Task<CourseDto?> GetCourseAsync(int id)
    {
        return await _context.Courses.Where(c => c.Id == id).Select(c => new CourseDto(
            c.Id,
            c.Name,
            c.Code,
            c.Description,
            c.StartDate,
            c.EndDate
            )).FirstOrDefaultAsync();
    }

    public async Task<CourseDto> NewCourseAsync(NewCourseDto course)
    {
        var c = new Domain.Models.Entities.Course();
        c.Name = course.Name;
        c.Code = course.Code;
        c.Description = course.Description;
        c.StartDate = course.StartDate;
        c.EndDate = course.EndDate;
        ValidateCourse(c);
        await _context.Courses.AddAsync(c);
        await _context.SaveChangesAsync();
        return new CourseDto(c.Id, c.Name, c.Code, c.Description, c.StartDate, c.EndDate);
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        var entity = await _context.Courses.FindAsync(id);
        if (entity is null)
            throw new KeyNotFoundException($"Course with id {id} not found.");

        _context.Remove(entity);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<CourseDto> PatchCourseAsync(PatchCourseDto course)
    {
        var entity = await _context.Courses.FindAsync(course.Id);
        if (entity is null)
            throw new NotFoundException($"Course with id {course.Id} not found.");

        if (!string.IsNullOrWhiteSpace(course.Name))
            entity.Name = course.Name;

        if (!string.IsNullOrWhiteSpace(course.Code))
            entity.Code = course.Code;

        if (course.Description is not null)
            entity.Description = course.Description;

        if (course.StartDate.HasValue)
            entity.StartDate = course.StartDate.Value;

        if (course.EndDate.HasValue)
            entity.EndDate = course.EndDate.Value;

        ValidateCourse(entity);

        await _context.SaveChangesAsync();

        return new CourseDto(entity.Id, entity.Name, entity.Code, entity.Description, entity.StartDate, entity.EndDate);
    }

    private static void ValidateCourse(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.Name))
            throw new CourseNameRequiredException("Name is required");
        if (course.StartDate > course.EndDate)
            throw new CourseInvalidDatesException("Invalid dates. Startdate must not be after Enddate");
    }
}
