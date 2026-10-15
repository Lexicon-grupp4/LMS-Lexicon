using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<INotificationService> _notificationService;
    private readonly ICourseService _courseService;
    private readonly IDashboardService _dashboardService;

    public IAuthService AuthService => _authService.Value;
    public IUserService UserService => _userService.Value;
    public INotificationService NotificationService => _notificationService.Value;
    public ICourseService CourseService => _courseService;
    public IDashboardService DashboardService => _dashboardService;

    public ServiceManager(
        Lazy<IAuthService> authService,
        Lazy<IUserService> userService,
        Lazy<INotificationService> notificationService,
        ICourseService courseService,
        IDashboardService dashboardService)
    {
        _authService = authService;
        _userService = userService;
        _notificationService = notificationService;
        _courseService = courseService;
        _dashboardService = dashboardService;
    }
}