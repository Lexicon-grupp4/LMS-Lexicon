using LMS.Shared.DTOs.SubmissionDtos;

namespace Service.Contracts;

public interface ISubmissionService
{
    Task<SubmissionDto> SubmitAsync(UploadSubmissionDto dto, Stream content, string userId);
    Task<SubmissionDto> GetMySubmissionAsync(int activityId, string userId);
}
