namespace SkillTracker.Services.Profile.API.Entities;

   // [DynamoDBTable("st-skill")]
    public class SkillEntity : EntityBase
    {
        public string EmpId { get; set; }

        public int SkillId { get; set; }

        public bool IsTechnical { get; set; }

        public string Name { get; set; }

        public int Proficiency { get; set; }
    }

