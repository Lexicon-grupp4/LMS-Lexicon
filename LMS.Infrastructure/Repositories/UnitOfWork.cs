using Domain.Contracts;
using LMS.Infrastructure.Data;

namespace LMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private Lazy<IUserRepository> _userRepsoitory;
    private Lazy<IModuleRepository> _moduleRepository;
    public IUserRepository UserRepsoitory => _userRepsoitory.Value;
    public IModuleRepository ModuleRepository => _moduleRepository.Value;
    public UnitOfWork(ApplicationDbContext context, Lazy<IUserRepository> userRepsoitory, Lazy<IModuleRepository> moduleRepository)
    {

        _context = context;
        _userRepsoitory = userRepsoitory;
        _moduleRepository = moduleRepository;

    }
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
}
