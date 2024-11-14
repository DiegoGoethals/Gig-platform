using Gig.Platform.Core.Interfaces.Services;
using Gig_Platform.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gig_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : Controller
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
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
        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> Create(ApplicationRequestDto applicationRequestDto)
        {
            var result = await _applicationService.CreateAsync(applicationRequestDto.JobId, applicationRequestDto.CandidateId);
            if (result.IsSucces)
            {
                return CreatedAtAction(nameof(Create), new { result.Value.Id }, new ApplicationResponseDto
                {
                    Id = result.Value.Id,
                    JobId = result.Value.JobId,
                    CandidateId = result.Value.CandidateId,
                    ApplicationStatus = result.Value.Status.Name
                });
            }
            return HandleError(result.Errors);
        }

        [HttpGet("job/{jobId}")]
        [Authorize(Policy = "Employer")]
        public async Task<IActionResult> GetAllByJob(Guid jobId)
        {
            var result = await _applicationService.GetAllByJobAsync(jobId);
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(a => new ApplicationResponseDto
                {
                    Id = a.Id,
                    JobId = a.JobId,
                    CandidateId = a.CandidateId,
                    CandidateName = a.Candidate.UserName,
                    ApplicationStatus = a.Status.Name
                }));
            }
            return HandleError(result.Errors);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _applicationService.DeleteAsync(id);
            if (result.IsSucces)
            {
                return Ok("Application removed!");
            }
            return HandleError(result.Errors);
        }

        [HttpGet("candidate/{candidateId}")]
        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> GetAllByCandidate(Guid candidateId)
        {
            var result = await _applicationService.GetAllByCandidateAsync(candidateId);
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(a => new ApplicationResponseDto
                {
                    Id = a.Id,
                    JobId = a.JobId,
                    CandidateId = a.CandidateId,
                    ApplicationStatus = a.Status.Name
                }));
            }
            return HandleError(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _applicationService.GetByIdAsync(id);
            if (result.IsSucces)
            {
                return Ok(new ApplicationResponseDto
                {
                    Id = result.Value.Id,
                    JobId = result.Value.JobId,
                    CandidateId = result.Value.CandidateId,
                    CandidateName = result.Value.Candidate.UserName,
                    ApplicationStatus = result.Value.Status.Name
                });
            }
            return HandleError(result.Errors);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> Update(Guid id, ApplicationRequestDto applicationRequestDto)
        {
            var result = await _applicationService.UpdateAsync(id);
            if (result.IsSucces)
            {
                return Ok(new ApplicationResponseDto
                {
                    Id = result.Value.Id,
                    JobId = result.Value.JobId,
                    CandidateId = result.Value.CandidateId,
                    ApplicationStatus = result.Value.Status.Name
                });
            }
            return HandleError(result.Errors);
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var result = await _applicationService.GetAllStatusesAsync();
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(s => new ApplicationStatusDto
                {
                    Id = s.Id,
                    Name = s.Name
                }));
            }
            return HandleError(result.Errors);
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "Employer")]
        public async Task<IActionResult> HandleApplication(Guid id, ApplicationStatusDto applicationStatusDto)
        {
            var entity = await _applicationService.MapDtoToEntity(applicationStatusDto.Id);
            var result = await _applicationService.HandleApplication(id, entity);
            if (result.IsSucces)
            {
                return Ok(new HandleStatusDto
                {
                    Job = result.Value.Job.Name,
                    JobId = result.Value.Job.Id,
                    Status = result.Value.Status.Name
                });
            }
            return HandleError(result.Errors);
        }
    }
}
