namespace SkillTracker.Services.Admin.API.Application.Commands;
public class CacheProfileCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string EmpId { get; set; }
        public string Mobile { get; set; }

        public List<Skill> Skills { get; set; }
    }
