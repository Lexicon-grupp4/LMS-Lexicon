namespace Domain.Contracts;

public interface IUnitOfWork
{
    IUserRepository UserRepsoitory { get; }
    IModuleRepository ModuleRepository { get; }
    IActivityRepository ActivityRepository { get; }
    IActionTypeRepository ActionTypeRepository { get; }
    Task<int> CompleteAsync();
}