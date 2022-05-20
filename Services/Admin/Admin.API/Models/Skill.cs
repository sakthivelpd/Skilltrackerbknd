namespace SkillTracker.Services.Admin.API.Models;
public class Skill
    {
    public bool IsTechnical { get; set; }

    [Required(ErrorMessage = "Skill Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Proficiency is required")]
    [Range(1, 20, ErrorMessage = "Invalid Proficiency")]
    public int Proficiency { get; set; }
}

