namespace Service.Contracts;

public interface IServiceManager
{
    IAuthService AuthService { get; }
    ICourseService CourseService { get; }
    INotificationService NotificationService { get; }
}