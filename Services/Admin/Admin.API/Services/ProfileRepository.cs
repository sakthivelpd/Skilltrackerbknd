namespace SkillTracker.Services.Admin.API.Services;
public class ProfileRepository : IProfileRepository
{
    private readonly SkillTrackerContext _context;

    public ProfileRepository(SkillTrackerContext dbcontext)
    {
        this._context = dbcontext;
    }

    public async Task<ProfileEntity> SearchByEmpIdAsync(string empId)
    {
        return _context.Profile.AsEnumerable().FirstOrDefault(s => s.EmpId.ToLower() == empId.ToLower());
    }

    public async Task<List<ProfileEntity>> GetProfilesByname(string name)
    {
        var pro = _context.Profile;
        Console.Write(_context.Profile.ToList());

        return _context.Profile.AsEnumerable().Where(s => s.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<IEnumerable<ProfileEntity>> SearchBySkillName(string skillName)
    {
        return _context.Profile.AsEnumerable().Where(company => company.skills.Any(user => user.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase)));
    }

    public async Task<List<ProfileEntity>> GetProfiles()
    {
        return _context.Profile.ToList();
    }
}

