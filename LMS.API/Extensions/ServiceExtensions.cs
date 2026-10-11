using Domain.Contracts;
using Domain.Models.ReadModels;
using LMS.Infrastructure.Repositories;
using LMS.Infrastructure.Services;
using LMS.Services;
using Service.Contracts;

namespace LMS.API.Extensions;

public static class ServiceExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddLazy<IUserRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
    }

    public static void AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService,SmtpEmailService>();
            
            
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddLazy<IAuthService>();
        services.AddLazy<IUserService>();

        services.AddScoped<INotificationService, NotificationService>();
        services.AddLazy<INotificationService>();
    }
}