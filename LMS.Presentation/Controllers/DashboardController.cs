using LMS.Shared.DTOs.DashboardDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System.Security.Claims;

namespace LMS.Presentation.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "Student")]
public class DashboardController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public DashboardController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("student")]
    public async Task<ActionResult<DashboardDto>> GetStudentDashboard()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var dashboard = await _serviceManager.DashboardService
            .GetStudentDashboardAsync(userId);

        if (dashboard is null)
            return NotFound();

        return Ok(dashboard);
    }
}