namespace Domain.Contracts;

public interface IUnitOfWork
{
    IUserRepository UserRepsoitory { get; }
    IModuleRepository ModuleRepository { get; }
    Task<int> CompleteAsync();
}