using LMS.Shared.DTOs.CourseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
namespace LMS.Presentation.Controllers;

[Route("api/course")]
[Authorize]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class CourseController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Courses",
        Description = "Courses desc")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(CourseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "unauthorized")]
    public async Task<ActionResult<CourseDto>> GetCourses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var user = await _serviceManager.UserService.GetUserAsync(userId);

        if (user.Role == "Teacher")
        {
            var courses = await _serviceManager.CourseService.GetCoursesAsync();
            return Ok(courses);
        }

        if (user.Role == "Student" && user.CourseId.HasValue)
        {
            var course = await _serviceManager.CourseService.GetCourseAsync(user.CourseId.Value);
            return Ok(course);
        }

        return Forbid();
    }

    [HttpGet("id")]
    [SwaggerOperation(
        Summary = "Course",
        Description = "Course desc")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(CourseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "unauthorized")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var user = await _serviceManager.UserService.GetUserAsync(userId);

        if (user.Role == "Student")
        {
            if (!user.CourseId.HasValue || user.CourseId.Value != id)
                return Forbid();
        }

        if (user.Role != "Student" && user.Role != "Teacher")
            return Forbid();

        var course = await _serviceManager.CourseService.GetCourseAsync(id);
        return Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(
        Summary = "New course",
        Description = "New course desc")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(CourseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "unauthorized")]
    public async Task<ActionResult<CourseDto>> NewCourse(NewCourseDto course)
    {
        var ret = await _serviceManager.CourseService.NewCourseAsync(course);
        return Ok(ret);
    }
}

