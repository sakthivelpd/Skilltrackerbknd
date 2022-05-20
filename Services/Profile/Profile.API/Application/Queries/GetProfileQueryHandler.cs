

namespace Profile.Application.Features.Queries.GetProfile
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileVM>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ILogger<GetProfileQueryHandler> _logger;

        public GetProfileQueryHandler(ILogger<GetProfileQueryHandler> logger, IProfileRepository profileRepository)
        {
            _logger = logger;
            _profileRepository = profileRepository;
        }
        public async Task<ProfileVM> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var profile =  _profileRepository.GetProfile(request.EmpId);

            return new ProfileVM
            {
                EmpId = profile.EmpId,
                Name = profile.Name,
                Email = profile.Email,
                Mobile = profile.Mobile,
                Skills = profile.skills

            };
        }
    }
}
