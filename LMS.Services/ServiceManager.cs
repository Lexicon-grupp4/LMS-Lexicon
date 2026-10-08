using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<INotificationService> _notificationService;

    public IAuthService AuthService => _authService.Value;
    public INotificationService NotificationService => _notificationService.Value;

    public ServiceManager(
        Lazy<IAuthService> authService,
        Lazy<INotificationService> notificationService)
    {
        _authService = authService;
        _notificationService = notificationService;
    }
}