using Domain.Contracts;
using LMS.Infrastructure.Data;

namespace LMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IUserRepository> _userRepsoitory;
    private readonly Lazy<IModuleRepository> _moduleRepository;
    private readonly Lazy<IDocumentRepository> _documentRepository;
    private Lazy<IActivityRepository> _activityRepository;
    private Lazy<IActionTypeRepository> _actionTypeRepository;
    public IUserRepository UserRepsoitory => _userRepsoitory.Value;
    public IModuleRepository ModuleRepository => _moduleRepository.Value;
    public IActivityRepository ActivityRepository => _activityRepository.Value;
    public IActionTypeRepository ActionTypeRepository => _actionTypeRepository.Value;
    public IDocumentRepository DocumentRepository => _documentRepository.Value;

    public UnitOfWork(ApplicationDbContext context, Lazy<IUserRepository> userRepsoitory, Lazy<IModuleRepository> moduleRepository, Lazy<IActivityRepository> activityRepository, Lazy<IActionTypeRepository> actionTypeRepository, Lazy<IDocumentRepository> documentRepository)
    {
        _context = context;
        _userRepsoitory = userRepsoitory;
        _moduleRepository = moduleRepository;
        _activityRepository = activityRepository;
        _actionTypeRepository = actionTypeRepository;
       _documentRepository = documentRepository;

    }
    

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
}
