namespace SkillTracker.Services.Profile.API.Application.Commands;
public class AddProfileCommandHandler : IRequestHandler<AddProfileCommand, string>
{
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<AddProfileCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    public AddProfileCommandHandler(
         ILogger<AddProfileCommandHandler> logger,
         IProfileRepository profileRepository,
         IPublishEndpoint publishEndpoint
      )
    {
        _logger = logger;
        _profileRepository = profileRepository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<string> Handle(AddProfileCommand request, CancellationToken cancellationToken)
    {
        var userId1 = await SaveProfileInfo(request);
        var userId = SavePersonalInfo(request);
        SaveSkills(request);

        _logger.LogInformation($"Profile {request.EmpId} is successfully created.");

        var eventMessage = new AddProfileEvent
        {
            Data = JsonSerializer.Serialize(request)
        };
       // await _publishEndpoint.Publish<AddProfileEvent>(eventMessage);

        return userId;
    }
    private async Task<string> SaveProfileInfo(AddProfileCommand request)
    {

        var profileInfo = new ProfileEntity();
        profileInfo.Name = request.Name;
        profileInfo.Email=request.Email;
        profileInfo.Mobile = request.Mobile;
        profileInfo.EmpId = request.EmpId;
        profileInfo.UserId = $"user{profileInfo.EmpId.ToUpper().Replace("CTS", "")}";
        profileInfo.CreatedDate = System.DateTime.UtcNow;
        profileInfo.LastModifiedDate = System.DateTime.UtcNow;
        profileInfo.skills =request.Skills;

        await _profileRepository.SaveProfile(profileInfo);

        return profileInfo.UserId;
    }

    private string SavePersonalInfo(AddProfileCommand request)
    {
        var personalInfo = new PersonalInfoEntity();
        personalInfo.Name = request.Name;
        personalInfo.Email = request.Email;
        personalInfo.Mobile = request.Mobile;
        personalInfo.EmpId = request.EmpId;       
        personalInfo.UserId = $"user{personalInfo.EmpId.ToUpper().Replace("CTS", "")}";
        personalInfo.CreatedDate = System.DateTime.UtcNow;
        personalInfo.LastModifiedDate = System.DateTime.UtcNow;

        return personalInfo.UserId;
    }

    private void SaveSkills(AddProfileCommand request)
    {
        var skills = request.Skills.Select(s => new SkillEntity
        {
            Name = s.Name,
            IsTechnical = s.IsTechnical,
            Proficiency = s.Proficiency,
            EmpId = request.EmpId,
            CreatedDate = System.DateTime.UtcNow,
            LastModifiedDate = System.DateTime.UtcNow
        }).ToList();
      
    }
}
