using Domain.Contracts;
using LMS.Infrastructure.Data;

namespace LMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private Lazy<IUserRepository> _userRepsoitory;
    private Lazy<IModuleRepository> _moduleRepository;
    private Lazy<IActivityRepository> _activityRepository;
    private Lazy<IActionTypeRepository> _actionTypeRepository;
    public IUserRepository UserRepsoitory => _userRepsoitory.Value;
    public IModuleRepository ModuleRepository => _moduleRepository.Value;
    public IActivityRepository ActivityRepository => _activityRepository.Value;
    public IActionTypeRepository ActionTypeRepository => _actionTypeRepository.Value;
    public UnitOfWork(ApplicationDbContext context, Lazy<IUserRepository> userRepsoitory, Lazy<IModuleRepository> moduleRepository, Lazy<IActivityRepository> activityRepository, Lazy<IActionTypeRepository> actionTypeRepository)
    {

        _context = context;
        _userRepsoitory = userRepsoitory;
        _moduleRepository = moduleRepository;
        _activityRepository = activityRepository;
        _actionTypeRepository = actionTypeRepository;
    }
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
}
