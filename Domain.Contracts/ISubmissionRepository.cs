using Domain.Models.Entities;

namespace Domain.Contracts;

public interface ISubmissionRepository : IRepositoryBase<Submission>
{
    Task<Submission?> GetByIdAsync(int id);
    Task<Submission?> GetByActivityAndUserAsync(int activityId, string userId);
}
