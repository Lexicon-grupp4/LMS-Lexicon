using LMS.Shared.DTOs.CourseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers;

[Route("api/course")]
[AllowAnonymous]
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
        var courses = await _serviceManager.CourseService.GetCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("id")]
    [SwaggerOperation(
        Summary = "Course",
        Description = "Course desc")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(CourseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "unauthorized")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var course = await _serviceManager.CourseService.GetCourseAsync(id);
        return Ok(course);
    }

    [HttpPost("new")]
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
