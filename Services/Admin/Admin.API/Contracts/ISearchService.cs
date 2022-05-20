namespace SkillTracker.Services.Admin.API.Contracts;
public interface ISearchService
{
    Task<IEnumerable<Profile>> Search(SearchProfileQuery query);
    Task<IEnumerable<Profile>> GetProfiles();
}
