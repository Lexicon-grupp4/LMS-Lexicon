using Domain.Contracts;
using LMS.Infrastructure.Data;

namespace LMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IUserRepository> _userRepsoitory;
    private readonly Lazy<IModuleRepository> _moduleRepository;
    private readonly Lazy<IDocumentRepository> _documentRepository;

    public IUserRepository UserRepsoitory => _userRepsoitory.Value;
    public IModuleRepository ModuleRepository => _moduleRepository.Value;
    public IDocumentRepository DocumentRepository => _documentRepository.Value;

    public UnitOfWork(ApplicationDbContext context, Lazy<IUserRepository> userRepsoitory, Lazy<IModuleRepository> moduleRepository, Lazy<IDocumentRepository> documentRepository)
    {
        _context = context;
        _userRepsoitory = userRepsoitory;
        _moduleRepository = moduleRepository;
        _documentRepository = documentRepository;
    }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
}
