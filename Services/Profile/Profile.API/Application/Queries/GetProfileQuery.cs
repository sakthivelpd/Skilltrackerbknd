namespace SkillTracker.Services.Profile.API.Application.Queries;

    public class GetProfileQuery: IRequest<ProfileVM>
    {
        public string EmpId { get; set; }
    }

