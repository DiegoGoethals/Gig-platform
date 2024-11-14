using Gig.Platform.Core.Interfaces.Services;
using Gig_Platform.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gig_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
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
        public async Task<IActionResult> Create(SkillRequestDto skillRequestDto)
        {
            var result = await _skillService.CreateAsync(skillRequestDto.Name);
            if (result.IsSucces)
            {
                return CreatedAtAction(nameof(Create), new { result.Value.Id }, new SkillResponseDto
                {
                    Id = result.Value.Id,
                    Name = result.Value.Name
                });
            }
            return HandleError(result.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _skillService.GetAllAsync();
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(skill => new SkillResponseDto
                {
                    Id = skill.Id,
                    Name = skill.Name
                }));
            }
            return HandleError(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _skillService.GetByIdAsync(id);
            if (result.IsSucces)
            {
                return Ok(new SkillResponseDto
                {
                    Id = result.Value.Id,
                    Name = result.Value.Name
                });
            }
            return HandleError(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SkillRequestDto skillRequestDto)
        {
            var result = await _skillService.UpdateAsync(id, skillRequestDto.Name);
            if (result.IsSucces)
            {
                return Ok(new SkillResponseDto
                {
                    Id = result.Value.Id,
                    Name = result.Value.Name
                });
            }
            return HandleError(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _skillService.DeleteAsync(id);
            if (result.IsSucces)
            {
                return Ok("Skill deleted successfully!");
            }
            return HandleError(result.Errors);
        }
    }
}
