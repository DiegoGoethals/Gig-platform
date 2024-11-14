namespace Gig_Platform.Dtos
{
    public class JobResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Salary { get; set; }
        public Guid EmployerId { get; set; }
        public ICollection<ApplicationResponseDto> Applications { get; set; }
        public ICollection<SkillResponseDto> Skills { get; set; }
    }
}
