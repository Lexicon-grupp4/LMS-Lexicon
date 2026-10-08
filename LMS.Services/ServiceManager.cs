using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IUserService> _userService;
    public IAuthService AuthService => _authService.Value;
    public IUserService UserService => _userService.Value;
    private readonly ICourseService _courseService;

    public ServiceManager(Lazy<IAuthService> authService, Lazy<IUserService> userService, CourseService courseService, Lazy<INotificationService> notificationService)
    {
        _authService = authService;
        _userService = userService;
        _courseService = courseService;
        _notificationService = notificationService;
    }
    private readonly Lazy<INotificationService> _notificationService;

    public ICourseService CourseService => _courseService;
    public INotificationService NotificationService => _notificationService.Value;

}