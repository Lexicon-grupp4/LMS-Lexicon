using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.DashboardDtos;
using LMS.Shared.DTOs.NotificationDtos;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;

namespace LMS.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto?> GetStudentDashboardAsync(string userId)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null || !user.CourseId.HasValue)
            return null;

        var courseId = user.CourseId.Value;
        var now = DateTime.Now;

        var course = await _context.Courses
            .AsNoTracking()
            .Where(c => c.Id == courseId)
            .Select(c => new CourseDto(
                c.Id,
                c.Name,
                c.Code,
                c.Description,
                c.StartDate,
                c.EndDate))
            .FirstOrDefaultAsync();

        if (course is null)
            return null;

        var moduleIds = await _context.Modules
            .AsNoTracking()
            .Where(m => m.CourseId == courseId)
            .Select(m => m.Id)
            .ToListAsync();

        var activityIds = await _context.Set<Activity>()
            .AsNoTracking()
            .Where(a => a.ModuleId.HasValue && moduleIds.Contains(a.ModuleId.Value))
            .Select(a => a.Id)
            .ToListAsync();

        var upcomingActivities = await _context.Set<Activity>()
            .AsNoTracking()
            .Where(a =>
               a.ModuleId.HasValue && moduleIds.Contains(a.ModuleId.Value) &&
                a.EndDate >= now)
            .OrderBy(a => a.StartDate)
            .Take(5)
            .Select(a => new DashboardActivityDto
            {
                Id = a.Id,
                Title = a.Title,
                ActionTypeName = a.ActionType.Name,
                StartDate = a.StartDate,
                EndDate = a.EndDate
            })
            .ToListAsync();

        var upcomingDeadlines = await _context.Set<Activity>()
            .AsNoTracking()
            .Where(a =>
               a.ModuleId.HasValue &&
             moduleIds.Contains(a.ModuleId.Value) &&
                a.EndDate >= now)
            .OrderBy(a => a.EndDate)
            .Take(5)

            .Select(a => new DashboardActivityDto
            {
                Id = a.Id,
                Title = a.Title,
                ActionTypeName = a.ActionType.Name,
                StartDate = a.StartDate,
                EndDate = a.EndDate
            })
            .ToListAsync();

        var recentDocuments = await _context.Set<Document>()
            .AsNoTracking()
            .Where(d =>
                d.CourseId == courseId ||
                (d.ModuleId.HasValue && moduleIds.Contains(d.ModuleId.Value)) ||
                (d.ActivityId.HasValue && activityIds.Contains(d.ActivityId.Value)))
            .OrderByDescending(d => d.UploadAt)
            .Take(5)
            .Select(d => new DashboardDocumentDto
            {
                Id = d.Id,
                FileName = d.StoredFile.FileName,
                UploadAt = d.UploadAt
            })
            .ToListAsync();

        var recentFeedback = await _context.Set<FeedBack>()
            .AsNoTracking()
            .Where(f => f.Submission.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .Take(5)
            .Select(f => new DashboardFeedbackDto
            {
                Id = f.Id,
                SubmissionId = f.SubmissionId,
                Comment = f.Comment,
                Grade = f.Grade,
                CreatedAt = f.CreatedAt
            })
            .ToListAsync();

        var recentNotifications = await _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead
            })
            .ToListAsync();

        return new DashboardDto
        {
            Course = course,
            UpcomingActivities = upcomingActivities,
            UpcomingDeadlines = upcomingDeadlines,
            RecentDocuments = recentDocuments,
            RecentFeedback = recentFeedback,
            RecentNotifications = recentNotifications
        };
    }
}
