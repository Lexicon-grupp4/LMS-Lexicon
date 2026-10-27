using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.SubmissionDtos;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using LMS.Shared.Validation;

namespace LMS.Services;

public class SubmissionService(IUnitOfWork uow, IMapper mapper, IConfiguration configuration) : ISubmissionService
{
    private readonly string _storageRoot = configuration["FileStorage:RootPath"] ?? Path.Combine(AppContext.BaseDirectory, "uploads");

    public async Task<SubmissionDto> SubmitAsync(UploadSubmissionDto dto, Stream content, string userId)
    {
        var error = FileRules.Validate(dto.FileName, dto.Size);
        if (error is not null) throw new InvalidFileException(error);

        // The student may only submit to an activity in their own course
        var activity = await uow.ActivityRepository.GetByIdWithModuleAsync(dto.ActivityId)
            ?? throw new NotFoundException($"Activity {dto.ActivityId} not found.");
        var user = await uow.UserRepsoitory.GetUserByIdAsync(userId)
            ?? throw new NotFoundException($"User {userId} not found.");

        if (activity.Module.CourseId != user.CourseId)
            throw new NotFoundException($"Activity {dto.ActivityId} not found.");

        var extension = Path.GetExtension(dto.FileName).ToLowerInvariant();
        var storageKey = $"{Guid.NewGuid()}{extension}";

        Directory.CreateDirectory(_storageRoot);
        await using (var fileStream = File.Create(Path.Combine(_storageRoot, storageKey)))
        {
            await content.CopyToAsync(fileStream);
        }

        var submittedAt = DateTime.UtcNow;
        var submission = new Submission
        {
            ActivityId = dto.ActivityId,
            UserId = userId,
            FileName = Path.GetFileName(dto.FileName),
            FilePath = storageKey,
            SubmittedAt = submittedAt,
            Status = submittedAt > activity.EndDate ? SubmissionStatus.Late : SubmissionStatus.Submitted
        };

        uow.SubmissionRepository.Create(submission);
        await uow.CompleteAsync();

        return mapper.Map<SubmissionDto>(submission);
    }

    public async Task<SubmissionDto?> GetMySubmissionAsync(int activityId, string userId) =>
        mapper.Map<SubmissionDto?>(
                await uow.SubmissionRepository.GetByActivityAndUserAsync(activityId, userId));

}
