namespace SkillTracker.Services.Profile.API.Application.Commands;
public class UpdateProfileCommand : IRequest<string>
    {
        public string EmpId { get; set; }

        public List<Skill> Skills { get; set; }
    }

