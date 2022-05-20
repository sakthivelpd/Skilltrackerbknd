namespace SkillTracker.Services.Profile.API.Services;
public class ProfileRepository : IProfileRepository
{
    private readonly SkillTrackerContext _context;

    public ProfileRepository(SkillTrackerContext dbcontext)
    {
        this._context = dbcontext;
    }

    public void DeleteProfile(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ProfileEntity>> GetAllProfiles(string id)
    {
        return _context.Profile.Where(s => s.EmpId == id).ToList();
    }
    
    public ProfileEntity GetProfile(string id)
    {
        return _context.Profile.FirstOrDefault(s => s.EmpId == id);
    }

    public async Task<ProfileEntity> SaveProfile(ProfileEntity profile)
    {
        await this._context.Profile.AddAsync(profile);
        await this._context.SaveChangesAsync();
        return profile;
    }

    public async Task<ProfileEntity> UpdateProfile(ProfileEntity profile)
    {
        ProfileEntity existingProfile = _context.Profile.FirstOrDefault(s => s.EmpId == profile.EmpId);      
        existingProfile.skills = profile.skills;
        existingProfile.LastModifiedDate = DateTime.Now;

        this._context.Update(existingProfile);
        await this._context.SaveChangesAsync();

        return profile;
    }
}

