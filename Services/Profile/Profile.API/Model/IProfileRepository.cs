namespace SkillTracker.Services.Profile.API.Models;
public interface IProfileRepository
    {
    Task<ProfileEntity> SaveProfile(ProfileEntity profile);
    Task<List<ProfileEntity>> GetAllProfiles(string id);
    ProfileEntity GetProfile(string id);
    void DeleteProfile(string id);
    Task<ProfileEntity> UpdateProfile(ProfileEntity profile);
}

