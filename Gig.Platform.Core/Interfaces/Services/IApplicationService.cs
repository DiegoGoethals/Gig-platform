using Gig.Platform.Core.Entities;
using Gig.Platform.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Interfaces.Services
{
    public interface IApplicationService
    {
        Task<ResultModel<Application>> CreateAsync(Guid jobId, Guid candidateId);
        Task<ResultModel<IEnumerable<Application>>> GetAllByJobAsync(Guid jobId);
        Task<ResultModel<Application>> DeleteAsync(Guid id);
        Task<ResultModel<IEnumerable<Application>>> GetAllByCandidateAsync(Guid candidateId);
        Task<ResultModel<Application>> GetByIdAsync(Guid id);
        Task<ResultModel<Application>> UpdateAsync(Guid id);
        Task<ResultModel<Application>> HandleApplication(Guid id, ApplicationStatus status);
        Task<ApplicationStatus> MapDtoToEntity(Guid Id);
        Task<ResultModel<IEnumerable<ApplicationStatus>>> GetAllStatusesAsync();
    }
}
