namespace Domain.Contracts;

public interface IUnitOfWork
{
    IUserRepository UserRepsoitory { get; }
    IModuleRepository ModuleRepository { get; }
    IDocumentRepository DocumentRepository { get; }
    Task<int> CompleteAsync();
}
