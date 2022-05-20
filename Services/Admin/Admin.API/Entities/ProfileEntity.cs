namespace SkillTracker.Services.Admin.API.Entities;
public class ProfileEntity : EntityBase
{
    [Key]
    public string EmpId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Mobile { get; set; }

    public string UserId { get; set; }

    public List<Skill> skills { get; set; }
}