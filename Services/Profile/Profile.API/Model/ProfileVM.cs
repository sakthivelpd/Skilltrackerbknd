namespace SkillTracker.Services.Profile.API.Models;
public class ProfileVM
    {
    public string EmpId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
    public string Mobile { get; set; }

    public List<Skill> Skills { get; set; }
}

