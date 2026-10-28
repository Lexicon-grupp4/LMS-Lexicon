using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class SubmissionRepository(ApplicationDbContext context) : RepositoryBase<Submission>(context), ISubmissionRepository
{
    public async Task<Submission?> GetByIdAsync(int id) =>
        await FindByCondition(s => s.Id == id)
        .Include(s => s.Activity)
        .FirstOrDefaultAsync();

    public async Task<Submission?> GetByActivityAndUserAsync(int activityId, string userId) =>
        await FindByCondition(s => s.ActivityId == activityId && s.UserId == userId)
        .FirstOrDefaultAsync();
}
