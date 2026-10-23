namespace Service.Contracts;

public interface IServiceManager
{
    IAuthService AuthService { get; }
    ICourseService CourseService { get; }
    IUserService UserService { get; }
    INotificationService NotificationService { get; }
    IModuleService ModuleService { get; }
    IDocumentService DocumentService { get; }

    IDashboardService DashboardService { get; }
}
