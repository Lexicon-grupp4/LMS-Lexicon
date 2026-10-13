using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<INotificationService> _notificationService;
    private readonly ICourseService _courseService;

    public IAuthService AuthService => _authService.Value;
    public IUserService UserService => _userService.Value;
    public INotificationService NotificationService => _notificationService.Value;
    public ICourseService CourseService => _courseService;

    public ServiceManager(
        Lazy<IAuthService> authService,
        Lazy<IUserService> userService,
        Lazy<INotificationService> notificationService,
        ICourseService courseService)
    {
        _authService = authService;
        _userService = userService;
        _notificationService = notificationService;
        _courseService = courseService;
    }
}