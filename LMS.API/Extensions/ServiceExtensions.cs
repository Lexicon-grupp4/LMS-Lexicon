using Domain.Contracts;
using LMS.Infrastructure.Repositories;
using LMS.Services;
using Service.Contracts;

namespace LMS.API.Extensions;

public static class ServiceExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
    }

    public static void AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddLazy<IAuthService>();

        services.AddScoped<INotificationService, NotificationService>();
        services.AddLazy<INotificationService>();
    }
}