using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.FeedbackDtos;
using Service.Contracts;

namespace LMS.Services;

public class FeedbackService(IUnitOfWork uow, IMapper mapper) : IFeedbackService
{
    public async Task<FeedbackDto> CreateAsync(int submissionId, CreateFeedbackDto dto, string teacherId)
    {
        _ = await uow.SubmissionRepository.GetByIdAsync(submissionId)
            ?? throw new NotFoundException($"Submission {submissionId} not found");
        var teacher = await uow.UserRepsoitory.GetUserByIdAsync(teacherId)
            ?? throw new NotFoundException($"User {teacherId} not found");

        var feedback = new FeedBack
        {
            SubmissionId = submissionId,
            UserId = teacherId,
            Comment = dto.Comment,
            Grade = dto.Grade,
            CreatedAt = DateTime.UtcNow
        };

        uow.FeedbackRepository.Create(feedback);
        // US11 #69: create notification for the student here, before CompleteAsync
        await uow.CompleteAsync();

        // Set after saving so EF does not try to insert the user
        feedback.User = teacher;
        return mapper.Map<FeedbackDto>(feedback);
    }
    public async Task<IEnumerable<FeedbackDto>> GetBySubmissionIdAsync(int submissionId) => mapper.Map<IEnumerable<FeedbackDto>>(
            await uow.FeedbackRepository.GetBySubmissionIdAsync(submissionId));

    public async Task<IEnumerable<FeedbackDto>> GetMyFeedbackAsync(int submissionId, string studentId)
    {
        var submission = await uow.SubmissionRepository.GetByIdAsync(submissionId);

        // Same 404 for "not found" and "not yours", so studetns can't probe othe submissions
        if (submission is null || submission.UserId != studentId)
            throw new NotFoundException($"Submission {submissionId} not found.");

        return await GetBySubmissionIdAsync(submissionId);
    }
}

