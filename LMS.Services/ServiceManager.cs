using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<INotificationService> _notificationService;
    private readonly Lazy<ICourseService> _courseService;
    private readonly Lazy<IModuleService> _moduleService;
    private readonly Lazy<IDocumentService> _documentService;
    private readonly IDashboardService _dashboardService;

    public IAuthService AuthService => _authService.Value;
    public IUserService UserService => _userService.Value;
    public INotificationService NotificationService => _notificationService.Value;
    public ICourseService CourseService => _courseService.Value;
    public IModuleService ModuleService => _moduleService.Value;
    public IDocumentService DocumentService => _documentService.Value;
    public IDashboardService DashboardService => _dashboardService;

    public ServiceManager(
            Lazy<IAuthService> authService,
            Lazy<IUserService> userService,
            Lazy<INotificationService> notificationService,
            Lazy<ICourseService> courseService,
            Lazy<IModuleService> moduleService,
            Lazy<IDocumentService> documentService,
            IDashboardService dashboardService)
    {
        _authService = authService;
        _userService = userService;
        _notificationService = notificationService;
        _courseService = courseService;
        _moduleService = moduleService;
        _documentService = documentService;
        _dashboardService = dashboardService;
    }
}
