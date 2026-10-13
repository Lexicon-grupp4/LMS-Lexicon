using Domain.Contracts;
using LMS.Shared.DTOs.NotificationDtos;
using Service.Contracts;

namespace LMS.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<IEnumerable<NotificationDto>> GetNotificationsAsync(string userId)
    {
        var notifications =
            await _notificationRepository.GetNotificationsByUserIdAsync(userId);

        return notifications.Select(notification => new NotificationDto
        {
            Id = notification.Id,
            Message = notification.Message,
            CreatedAt = notification.CreatedAt,
            IsRead = notification.IsRead
        });
    }

    public async Task<bool> MarkAsReadAsync(int notificationId, string userId)
    {
        var notification =
            await _notificationRepository.GetNotificationByIdAsync(
                notificationId,
                userId);

        if (notification is null)
            return false;

        if (notification.IsRead)
            return true;

        notification.IsRead = true;

        return await _notificationRepository.SaveChangesAsync() > 0;
    }
}