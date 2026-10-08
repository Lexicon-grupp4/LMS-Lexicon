using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly ICourseService _courseService;

    public IAuthService AuthService => _authService.Value;
    public ICourseService CourseService => _courseService;

    public ServiceManager(Lazy<IAuthService> authService, ICourseService courseService)
    {
        _authService = authService;
        _courseService = courseService;
    }
}
