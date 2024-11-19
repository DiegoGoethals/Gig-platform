namespace Gig.Platform.Core.Interfaces.Repositories
{
    public interface ISkillRepository : IBaseRepository<Skill>
    {
        Task<Skill> GetByName(string name);
    }
}
