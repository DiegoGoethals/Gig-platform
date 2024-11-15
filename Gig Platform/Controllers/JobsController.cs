using Gig.Platform.Core.Interfaces.Services;
using Gig.Platform.Core.Services;
using Gig_Platform.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gig_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly GeocodingService _geocodingService;

        public JobsController(IJobService jobService, GeocodingService geocodingService)
        {
            _jobService = jobService;
            _geocodingService = geocodingService;
        }

        private IActionResult HandleError(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
            return BadRequest(ModelState.Values);
        }

        [HttpPost]
        [Authorize(Policy = "Employer")]
        public async Task<IActionResult> Create(JobRequestDto jobRequestDto)
        {
            if (jobRequestDto.Latitude == null || jobRequestDto.Longitude == null)
            {
                var (latitude, longitude) = await _geocodingService.GeocodeAddressAsync(
                    jobRequestDto.StreetName,
                    jobRequestDto.HouseNumber,
                    jobRequestDto.PostalCode,
                    jobRequestDto.City
                );

                if (latitude == null || longitude == null)
                {
                    return BadRequest("Unable to geocode the provided address.");
                }

                jobRequestDto.Latitude = (double)latitude;
                jobRequestDto.Longitude = (double)longitude;
            }

            else if (string.IsNullOrWhiteSpace(jobRequestDto.StreetName) || string.IsNullOrWhiteSpace(jobRequestDto.City))
            {
                var geocodedAddress = await _geocodingService.ReverseGeocodeAsync(jobRequestDto.Latitude, jobRequestDto.Longitude);

                if (geocodedAddress == null)
                {
                    return BadRequest("Unable to reverse geocode the provided coordinates.");
                }

                jobRequestDto.StreetName = geocodedAddress.StreetName;
                jobRequestDto.HouseNumber = geocodedAddress.HouseNumber;
                jobRequestDto.PostalCode = geocodedAddress.PostalCode;
                jobRequestDto.City = geocodedAddress.City;

                // Validate extracted fields
                if (string.IsNullOrWhiteSpace(jobRequestDto.StreetName) ||
                        string.IsNullOrWhiteSpace(jobRequestDto.PostalCode) ||
                        string.IsNullOrWhiteSpace(jobRequestDto.City))
                {
                    return BadRequest("Insufficient address details from reverse geocoding.");
                }
            }

            var result = await _jobService.CreateAsync(
                jobRequestDto.Name,
                jobRequestDto.Description,
                jobRequestDto.Salary,
                jobRequestDto.EmployerId,
                jobRequestDto.Skills,
                jobRequestDto.Latitude,
                jobRequestDto.Longitude,
                jobRequestDto.StreetName,
                jobRequestDto.HouseNumber,
                jobRequestDto.PostalCode,
                jobRequestDto.City
            );

            if (result.IsSucces)
            {
                return CreatedAtAction(nameof(Create), new { result.Value.Id }, new JobResponseDto
                {
                    Id = result.Value.Id,
                    Name = result.Value.Name,
                    Description = result.Value.Description,
                    Salary = result.Value.Salary,
                    EmployerId = result.Value.EmployerId,
                    Skills = result.Value.Skills.Select(skill => new SkillResponseDto
                    {
                        Id = skill.Id,
                        Name = skill.Name
                    }).ToList(),
                    Latitude = result.Value.Latitude,
                    Longitude = result.Value.Longitude,
                    StreetName = result.Value.StreetName,
                    HouseNumber = result.Value.HouseNumber,
                    PostalCode = result.Value.PostalCode,
                    City = result.Value.City
                });
            }
            return HandleError(result.Errors);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Employer")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _jobService.DeleteAsync(id);
            if (result.IsSucces)
            {
                return Ok("Job removed!");
            }
            return HandleError(result.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _jobService.GetAllAsync();
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(job => new JobResponseDto
                {
                    Id = job.Id,
                    Name = job.Name,
                    Description = job.Description,
                    Salary = job.Salary,
                    EmployerId = job.EmployerId,
                    Skills = job.Skills.Select(skill => new SkillResponseDto
                    {
                        Id = skill.Id,
                        Name = skill.Name
                    }).ToList()
                }));
            }
            return HandleError(result.Errors);
        }

        [HttpGet("skills")]
        public async Task<IActionResult> GetBySkills([FromQuery] IEnumerable<string> skills)
        {
            var result = await _jobService.GetBySkills(skills);
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(job => new JobResponseDto
                {
                    Id = job.Id,
                    Name = job.Name,
                    Description = job.Description,
                    Salary = job.Salary,
                    EmployerId = job.EmployerId,
                    Skills = job.Skills.Select(skill => new SkillResponseDto
                    {
                        Id = skill.Id,
                        Name = skill.Name
                    }).ToList()
                }));
            }
            return HandleError(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _jobService.GetByIdAsync(id);
            if (result.IsSucces)
            {
                return Ok(new JobResponseDto
                {
                    Id = result.Value.Id,
                    Name = result.Value.Name,
                    Description = result.Value.Description,
                    Salary = result.Value.Salary,
                    EmployerId = result.Value.EmployerId,
                    Skills = result.Value.Skills.Select(skill => new SkillResponseDto
                    {
                        Id = skill.Id,
                        Name = skill.Name
                    }).ToList()
                });
            }
            return HandleError(result.Errors);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Employer")]
        public async Task<IActionResult> Update(Guid id, JobRequestDto jobRequestDto)
        {
            var result = await _jobService.UpdateAsync(id, jobRequestDto.Name, jobRequestDto.Description, jobRequestDto.Salary, jobRequestDto.Skills);
            if (result.IsSucces)
            {
                return Ok(new JobResponseDto
                {
                    Id = result.Value.Id,
                    Name = result.Value.Name,
                    Description = result.Value.Description,
                    Salary = result.Value.Salary,
                    EmployerId = result.Value.EmployerId,
                    Skills = result.Value.Skills.Select(skill => new SkillResponseDto
                    {
                        Id = skill.Id,
                        Name = skill.Name
                    }).ToList()
                });
            }
            return HandleError(result.Errors);
        }

        [HttpGet("employer/{employerId}")]
        [Authorize(Policy = "Employer")]
        public async Task<IActionResult> GetAllByEmployer(Guid employerId)
        {
            var result = await _jobService.GetAllByEmployerAsync(employerId);
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(job => new JobResponseDto
                {
                    Id = job.Id,
                    Name = job.Name,
                    Description = job.Description,
                    Salary = job.Salary,
                    EmployerId = job.EmployerId,
                    Applications = job.Applications.Select(application => new ApplicationResponseDto
                    {
                        Id = application.Id,
                        CandidateId = application.CandidateId,
                        CandidateName = application.Candidate.UserName,
                        ApplicationStatus = application.Status.Name
                    }).ToList(),
                    Skills = job.Skills.Select(skill => new SkillResponseDto
                    {
                        Id = skill.Id,
                        Name = skill.Name
                    }).ToList()
                }));
            }
            return HandleError(result.Errors);
        }
    }
}
