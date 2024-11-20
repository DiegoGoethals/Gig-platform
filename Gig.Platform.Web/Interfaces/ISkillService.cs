global using Gig.Platform.Shared.Entities;
global using Gig.Platform.Shared.Dtos;

namespace Gig.Platform.Web.Interfaces
{
    public interface ISkillService
    {
        Task<IEnumerable<SkillResponseDto>> GetSkillsAsync();
    }
}
