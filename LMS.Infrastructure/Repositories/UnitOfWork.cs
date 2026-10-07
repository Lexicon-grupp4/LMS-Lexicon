using Domain.Contracts;
using LMS.Infrastructure.Data;

namespace LMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private Lazy<IUserRepository> _userRepsoitory;
    public IUserRepository UserRepsoitory => _userRepsoitory.Value;
    public UnitOfWork(ApplicationDbContext context, Lazy<IUserRepository> userRepsoitory)
    {
        _context = context;
        _userRepsoitory = userRepsoitory;

    }
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
}
