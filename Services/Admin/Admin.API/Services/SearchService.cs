namespace SkillTracker.Services.Admin.API.Services;
public class SearchService : ISearchService
{
    private readonly IProfileRepository _profileRepository;

    private readonly ILogger<SearchService> _logger;
    private readonly IRedisCacheService _redisCacheService;
    private readonly bool _cacheEnabled;

    public SearchService(

        IRedisCacheService redisCacheService,
        ILogger<SearchService> logger,
        IConfiguration configuration,
        IProfileRepository profileRepository
    )
    {

        _redisCacheService = redisCacheService;
        _logger = logger;
        _profileRepository = profileRepository;

        bool.TryParse(configuration["CacheEnabled"], out _cacheEnabled);
    }
    public async Task<IEnumerable<Profile>> Search(SearchProfileQuery query)
    {
        if (!string.IsNullOrWhiteSpace(query.EmpId))
        {
            var response = new List<Profile>();
            var profile = await SearchById(query.EmpId);
            if (profile != null)
            {
                response.Add(profile);
            }
            return response;
        }
        else if (!string.IsNullOrWhiteSpace(query.Name))
        {
            return await SearchByName(query.Name);
        }
        else if (!string.IsNullOrWhiteSpace(query.Skill))
        {
            return await SearchBySkillName(query.Skill);
        }
        return new List<Profile>();
    }

    private async Task<Profile> SearchById(string empId)
    {
        var cachedData = GetFromCache(empId);
        if (cachedData != null)
        {
            return cachedData;
        }

        var profile = MapToProfile(await _profileRepository.SearchByEmpIdAsync(empId));

        if (profile != null)
        {
            await SetToCache(empId, profile);
            return profile;
        }
        return null;
    }

    private Profile MapToProfile(ProfileEntity profileEntity)
    {
        if(profileEntity !=null)
        return new Profile
        {
            EmpId =profileEntity.EmpId,
            Name=profileEntity.Name,
            Email=profileEntity.Email,
            Mobile=profileEntity.Mobile,
            Skills=profileEntity.skills
        };
        else
        {
            return new Profile();
        }
    }

    private async Task<IEnumerable<Profile>> SearchByName(string name)
    {

        var profileData = await _profileRepository.GetProfilesByname(name);

       
        var plist = profileData.Select(x => new Profile
        {
            Name = x.Name,
            Email = x.Email,
            Mobile = x.Mobile,
            EmpId = x.EmpId,
            Skills = x.skills
        });
        return plist;
    }

    private async Task<IEnumerable<Profile>> SearchBySkillName(string skillName)
    {
        var profileData = await _profileRepository.SearchBySkillName(skillName);
        var plist = profileData.Select(x => new Profile
        {
            Name = x.Name,
            Email = x.Email,
            Mobile = x.Mobile,
            EmpId = x.EmpId,
            Skills = x.skills
        });
        return plist;
        //return profiles;
    }

    private Profile? GetFromCache(string key)
    {
        return _cacheEnabled ? _redisCacheService.Get<Profile>(key) : null;
    }

    private async Task SetToCache(string key, Profile data)
    {
        if (_cacheEnabled)
        {
          
            Task.FromResult( _redisCacheService.Set<Profile>(key, data));
        }
    }

    public async Task<IEnumerable<Profile>> GetProfiles()
    {
        var profileData = await _profileRepository.GetProfiles();
        var plist = profileData.Select(x => new Profile
        {
            Name = x.Name,
            Email = x.Email,
            Mobile = x.Mobile,
            EmpId = x.EmpId,
            Skills = x.skills
        });
        return plist;
    }
}

