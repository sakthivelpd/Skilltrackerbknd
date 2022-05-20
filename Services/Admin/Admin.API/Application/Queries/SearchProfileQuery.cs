
namespace SkillTracker.Services.Admin.API.Application.Queries;

public class SearchProfileQuery: IRequest<IEnumerable<Profile>>
    {
        public string EmpId { get; set; }

        public string Name { get; set; }

        public string Skill { get; set; }
    }

