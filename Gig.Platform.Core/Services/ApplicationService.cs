using Gig.Platform.Core.Interfaces.Repositories;
using Gig.Platform.Core.Interfaces.Services;
using Gig.Platform.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;

        public ApplicationService(IApplicationRepository applicationRepository, IJobRepository jobRepository, IApplicationStatusRepository applicationStatusRepository)
        {
            _applicationRepository = applicationRepository;
            _jobRepository = jobRepository;
            _applicationStatusRepository = applicationStatusRepository;
        }

        public async Task<ResultModel<Application>> CreateAsync(Guid jobId, Guid candidateId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null)
            {
                return new ResultModel<Application>
                {
                    IsSucces = false,
                    Errors = new List<string> { "Job not found" }
                };
            }

            var application = new Application
            {
                Id = Guid.NewGuid(),
                JobId = jobId,
                CandidateId = candidateId,
                Status = _applicationStatusRepository.GetAll().First(s => s.Name == "Pending")
            };

            if (await _applicationRepository.AddAsync(application))
            {
                return new ResultModel<Application>
                {
                    Value = application,
                    IsSucces = true
                };
            }
            return new ResultModel<Application>
            {
                IsSucces = false,
                Errors = new List<string> { "Error creating application" }
            };
        }

        public async Task<ResultModel<IEnumerable<Application>>> GetAllByJobAsync(Guid jobId)
        {
            var applications = await _applicationRepository.GetAllByJobAsync(jobId);
            if (applications.Any())
            {
                return new ResultModel<IEnumerable<Application>>
                {
                    Value = applications,
                    IsSucces = true
                };
            }
            return new ResultModel<IEnumerable<Application>>
            {
                IsSucces = true,
                Value = new List<Application> { }
            };
        }

        public async Task<ResultModel<Application>> DeleteAsync(Guid id)
        {
            var application = await _applicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                return new ResultModel<Application>
                {
                    IsSucces = false,
                    Errors = new List<string> { "Application not found" }
                };
            }
            if (await _applicationRepository.DeleteAsync(application))
            {
                return new ResultModel<Application>
                {
                    IsSucces = true
                };
            }
            return new ResultModel<Application>
            {
                IsSucces = false,
                Errors = new List<string> { "Error deleting application" }
            };
        }

        public async Task<ResultModel<IEnumerable<Application>>> GetAllByCandidateAsync(Guid candidateId)
        {
            var applications = await _applicationRepository.GetAllByCandidateAsync(candidateId);
            if (applications.Any())
            {
                return new ResultModel<IEnumerable<Application>>
                {
                    Value = applications,
                    IsSucces = true
                };
            }
            return new ResultModel<IEnumerable<Application>>
            {
                IsSucces = true,
                Value = new List<Application> { }
            };
        }

        public async Task<ResultModel<Application>> GetByIdAsync(Guid id)
        {
            var application = await _applicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                return new ResultModel<Application>
                {
                    IsSucces = false,
                    Errors = new List<string> { "Application not found" }
                };
            }
            return new ResultModel<Application>
            {
                Value = application,
                IsSucces = true
            };
        }

        public async Task<ResultModel<Application>> UpdateAsync(Guid id)
        {
            var application = await _applicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                return new ResultModel<Application>
                {
                    IsSucces = false,
                    Errors = new List<string> { "Application not found" }
                };
            }
            if (await _applicationRepository.UpdateAsync(application))
            {
                return new ResultModel<Application>
                {
                    Value = application,
                    IsSucces = true
                };
            }
            return new ResultModel<Application>
            {
                IsSucces = false,
                Errors = new List<string> { "Error updating application" }
            };
        }

        public async Task<ApplicationStatus> MapDtoToEntity(Guid Id)
        {
            return await _applicationStatusRepository.GetByIdAsync(Id);
        }

        public async Task<ResultModel<Application>> HandleApplication(Guid id, ApplicationStatus status)
        {
            var application = await _applicationRepository.GetByIdAsync(id);
            var statusExists = _applicationStatusRepository.GetAll().Any(s => s == status);
            if (application == null || !statusExists)
            {
                return new ResultModel<Application>
                {
                    IsSucces = false,
                    Errors = new List<string> { "Application not found" }
                };
            }
            application.Status = status;
            if (await _applicationRepository.UpdateAsync(application))
            {
                return new ResultModel<Application>
                {
                    Value = application,
                    IsSucces = true
                };
            }
            return new ResultModel<Application>
            {
                IsSucces = false,
                Errors = new List<string> { "Error handling application" }
            };
        }

        public async Task<ResultModel<IEnumerable<ApplicationStatus>>> GetAllStatusesAsync()
        {
            var statuses = await _applicationStatusRepository.GetAllAsync();
            if (statuses.Any())
            {
                return new ResultModel<IEnumerable<ApplicationStatus>>
                {
                    Value = statuses,
                    IsSucces = true
                };
            }
            return new ResultModel<IEnumerable<ApplicationStatus>>
            {
                IsSucces = false,
                Errors = new List<string> { "No statuses found" }
            };
        }
    }
}
