using System.Security.Claims;
using LMS.Shared.DTOs.CourseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers;

[Route("api/course")]
[Authorize]
[ApiController]
[Route("api/course")]
[Consumes("application/json")]
[Produces("application/json")]
public class CourseController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpGet]
    [SwaggerOperation(Summary = "Courses", Description = "Courses desc")]
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

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Course", Description = "Course desc")]
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

    [HttpGet]
    [SwaggerOperation(Summary = "Course", Description = "A students course")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(CourseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "unauthorized")]
    public async Task<ActionResult<CourseDto>> GetCourse()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var user = await _serviceManager.UserService.GetUserAsync(userId);
        if (user.Role == "Student" && user.CourseId.HasValue)
        {
            var course = await _serviceManager.CourseService.GetCourseAsync(user.CourseId.Value);
            return Ok(course);
        }
        return Forbid();
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    [SwaggerOperation(Summary = "New course", Description = "New course desc")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(CourseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "unauthorized")]
    public async Task<ActionResult<CourseDto>> NewCourse(NewCourseDto course)
    {
        var ret = await _serviceManager.CourseService.NewCourseAsync(course);
        return Ok(ret);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Delete course", Description = "Delete course desc")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(CourseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "unauthorized")]
    public async Task<ActionResult<bool>> DeleteCourse(int id)
    {
        var ret = await _serviceManager.CourseService.DeleteCourseAsync(id);
        return Ok(ret);
    }

    [HttpPatch]
    [SwaggerOperation(Summary = "Patch course", Description = "Patch course desc")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(CourseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "unauthorized")]
    public async Task<ActionResult<CourseDto>> PatchCourse(PatchCourseDto course)
    {
        var ret = await _serviceManager.CourseService.PatchCourseAsync(course);
        return Ok(ret);
    }
}
