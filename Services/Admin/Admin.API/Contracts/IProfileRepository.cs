public interface IProfileRepository
{
    Task<ProfileEntity> SearchByEmpIdAsync(string empId);

    Task<List<ProfileEntity>> GetProfilesByname(string name);

    Task<IEnumerable<ProfileEntity>> SearchBySkillName(string skillName);
    Task<List<ProfileEntity>> GetProfiles();
}
