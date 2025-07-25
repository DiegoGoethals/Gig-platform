﻿namespace Gig.Platform.Shared.Dtos
{
    public class JobResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Salary { get; set; }
        public Guid EmployerId { get; set; }
        public string EmployerName { get; set; }
        public ICollection<ApplicationResponseDto> Applications { get; set; }
        public ICollection<SkillResponseDto> Skills { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}