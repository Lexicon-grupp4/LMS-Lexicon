using System.Security.Claims;
using LMS.Shared.DTOs.SubmissionDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Presentation.Controllers;

[Route("api/submissions")]
[Authorize(Roles = "Student")]
[ApiController]
public class SubmissionController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(11 * 1024 * 1024)]
    [SwaggerOperation(Summary = "Submit assignment", Description = "Max 10 MB. PDF, DOCX, PPTX, XLSX, TXT, JPG, PNG")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(SubmissionDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "invalid file")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "activity not found")]
    public async Task<ActionResult<SubmissionDto>> Submit(
        IFormFile file,
        [FromForm] int activityId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var dto = new UploadSubmissionDto(file.FileName, file.ContentType, file.Length, activityId);

        await using var stream = file.OpenReadStream();
        var result = await _serviceManager.SubmissionService.SubmitAsync(dto, stream, userId);
        return Ok(result);
    }

    [HttpGet("activity/{activityId:int}/mine")]
    [SwaggerOperation(Summary = "My submission", Description = "The logged-in student's submission for an activity")]
    [SwaggerResponse(StatusCodes.Status200OK, "successful", typeof(SubmissionDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "no submission")]
    public async Task<ActionResult<SubmissionDto>> GetMine(int activityId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var result = await _serviceManager.SubmissionService.GetMySubmissionAsync(activityId, userId);
        return result is null ? NotFound() : Ok(result);
    }
}
