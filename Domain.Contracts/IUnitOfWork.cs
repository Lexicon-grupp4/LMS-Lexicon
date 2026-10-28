namespace Domain.Contracts;

public interface IUnitOfWork
{
    IUserRepository UserRepsoitory { get; }
    IModuleRepository ModuleRepository { get; }
    IActivityRepository ActivityRepository { get; }
    IActionTypeRepository ActionTypeRepository { get; }
    IDocumentRepository DocumentRepository { get; }
    ISubmissionRepository SubmissionRepository { get; }
    INotificationRepository NotificationRepository { get; }
    Task<int> CompleteAsync();
}
