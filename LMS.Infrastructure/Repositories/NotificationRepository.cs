using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Create(Notification entity)
    {
        _context.Notifications.Add(entity);
    }

    public void Delete(Notification entity)
    {
        _context.Notifications.Remove(entity);
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId)
    {
        return await _context.Notifications
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Notification?> GetNotificationByIdAsync(
        int notificationId,
        string userId)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(notification =>
                notification.Id == notificationId &&
                notification.UserId == userId);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}