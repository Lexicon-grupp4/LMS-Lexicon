using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Extensions;
using LMS.Shared.Paging;
using Microsoft.EntityFrameworkCore;


namespace LMS.Infrastructure.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IPagedList<Activity>> GetAllAsync(
            QueryParameters parameters,
            bool trackChanges = false)
        {
            IQueryable<Activity> query = _context.Activities
                .Include(a => a.ActionType)
                .OrderBy(a => a.StartDate);

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.ToPagedListAsync(parameters);
        }

        // Get activities belonging to a specific module
        public async Task<IPagedList<Activity>> GetByModuleIdAsync(
            int moduleId,
            QueryParameters parameters,
            bool trackChanges = false)
        {
            IQueryable<Activity> query = _context.Activities
                .Where(a => a.ModuleId == moduleId)
                .Include(a => a.ActionType)
                .OrderBy(a => a.StartDate);

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.ToPagedListAsync(parameters);
        }

        public async Task<Activity?> GetByIdAsync(
            int id,
            bool trackChanges = false)
        {
            IQueryable<Activity> query = _context.Activities;

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Activity?> GetByIdWithModuleAsync(
            int activityId,
            bool trackChanges = false)
        {
            IQueryable<Activity> query = _context.Activities
                .Include(a => a.Module)
                .Include(a => a.ActionType);

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(
                a => a.Id == activityId);
        }

        public async Task<bool> HasOverlappingDatesAsync(
            int moduleId,
            DateTime startDate,
            DateTime endDate,
            int? excludeActivityId = null)
        {
            return await _context.Activities.AnyAsync(a =>
                a.ModuleId == moduleId &&
                (excludeActivityId == null ||
                 a.Id != excludeActivityId.Value) &&
                a.StartDate < endDate &&
                a.EndDate > startDate);
        }

        public async Task<bool> IsWithinModulePeriodAsync(
            int moduleId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.Modules.AnyAsync(m =>
                m.Id == moduleId &&
                startDate >= m.StartDate &&
                endDate <= m.EndDate);
        }
    }
}
