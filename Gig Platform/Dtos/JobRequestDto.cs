namespace Gig_Platform.Dtos
{
    public class JobRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Salary { get; set; }
        public Guid EmployerId { get; set; }
        public IEnumerable<string> Skills { get; set; }
    }
}
