using Domain.Contracts;
using LMS.Infrastructure.Repositories;
using LMS.Services;
using Service.Contracts;

namespace LMS.API.Extensions;

public static class ServiceExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IActionTypeRepository, ActionTypeRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddLazy<IModuleRepository>();
        services.AddLazy<IDocumentRepository>();
        services.AddLazy<ISubmissionRepository>();
        services.AddLazy<IUserRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddLazy<IActivityRepository>();
        services.AddLazy<IActionTypeRepository>();
    }

    public static void AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IActionTypeService, ActionTypeService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddLazy<IAuthService>();
        services.AddLazy<IUserService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddLazy<INotificationService>();
        services.AddLazy<ICourseService>();
        services.AddLazy<IModuleService>();
        services.AddLazy<IActivityService>();
        services.AddLazy<IActionTypeService>();

        services.AddScoped<IDocumentService, DocumentService>();
        services.AddLazy<IDocumentService>();
        services.AddLazy<ISubmissionService>();
    }
}
