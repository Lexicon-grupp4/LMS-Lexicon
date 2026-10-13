namespace Domain.Contracts;

public interface IUnitOfWork
{
    IUserRepository UserRepsoitory { get; }
    Task<int> CompleteAsync();
}