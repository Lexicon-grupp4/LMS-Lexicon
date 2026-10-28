using LMS.Shared.DTOs.SubmissionDtos;
using LMS.Shared.Paging;

namespace Service.Contracts;

public interface ISubmissionService
{
    Task<SubmissionDto> SubmitAsync(UploadSubmissionDto dto, Stream content, string userId);
    Task<SubmissionDto?> GetMySubmissionAsync(int activityId, string userId);
    Task<PagedResponse<TeacherAssignmentSubmissionDto>> GetTeacherSubmissionsAsync(
        string teacherId,
        int courseId,
        int? activityId,
        string? status,
        QueryParameters parameters);
    Task<(Stream Content, string FileName)?> DownloadSubmissionAsync(int submissionId, string teacherId);

}
