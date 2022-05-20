namespace SkillTracker.Services.Profile.API.Application.Commands;
public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, string>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ILogger<UpdateProfileCommandHandler> _logger;

        public UpdateProfileCommandHandler(ILogger<UpdateProfileCommandHandler> logger, IProfileRepository profileRepository)
        {
            _logger = logger;
            _profileRepository = profileRepository;
        }


        public async Task<string> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            ProfileEntity profileInfo = new ProfileEntity
            {
                EmpId = request.EmpId,              
                skills = request.Skills
            };
            await _profileRepository.UpdateProfile(profileInfo);

            _logger.LogInformation($"Profile {request.EmpId} is successfully updated.");

            return request.EmpId;
        }
    }

