namespace SkillTracker.Services.Admin.API.Application.Commands;
public class CacheProfileCommandHandler : IRequestHandler<CacheProfileCommand, string>
{
    private readonly IRedisCacheService _cacheRepo;
    private readonly ILogger<CacheProfileCommandHandler> _logger;

    private readonly bool _cacheEnabled;

    public CacheProfileCommandHandler(
        IRedisCacheService cacheRepo,
        ILogger<CacheProfileCommandHandler> logger,
        IConfiguration configuration)
    {
        _cacheRepo = cacheRepo;
        _logger = logger;

        bool.TryParse(configuration["CacheEnabled"], out _cacheEnabled);
    }

    public async Task<string> Handle(CacheProfileCommand request, CancellationToken cancellationToken)
    {
        if (_cacheEnabled)
        {
            var profile = new Profile();
            profile.Name = request.Name;
            profile.Email = request.Email;
            profile.Mobile = request.Mobile;
            profile.EmpId = request.EmpId;
            profile.Skills = request.Skills;

            await Task.FromResult(_cacheRepo.Set<object>(profile.EmpId, profile));

            _logger.LogInformation($"Profile {profile.EmpId} is successfully cached.");

            return profile.EmpId;
        }
        else
        {
            _logger.LogInformation($"Cached not enabled.");
            return request.EmpId;
        }
    }
}

