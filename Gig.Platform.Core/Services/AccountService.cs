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
    public class AccountService : IAccountService
    {
        private readonly IUserRepository<ApplicationUser> _userRepository;
        private readonly ISkillRepository _skillRepository;

        public AccountService(IUserRepository<ApplicationUser> userRepository, ISkillRepository skillRepository)
        {
            _userRepository = userRepository;
            _skillRepository = skillRepository;
        }

        public async Task<ResultModel<IEnumerable<ApplicationUser>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            if (users.Any())
            {
                return new ResultModel<IEnumerable<ApplicationUser>>
                {
                    IsSucces = true,
                    Value = users
                };
            }
            return new ResultModel<IEnumerable<ApplicationUser>>
            {
                IsSucces = false,
                Errors = new List<string> { "No users found" }
            };
        }

        public async Task<ResultModel<ApplicationUser>> GetByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user != null)
            {
                return new ResultModel<ApplicationUser>
                {
                    IsSucces = true,
                    Value = user
                };
            }
            return new ResultModel<ApplicationUser>
            {
                IsSucces = false,
                Errors = new List<string> { "User not found" }
            };
        }

        public async Task<ResultModel<ApplicationUser>> Register(ApplicationUser user, IEnumerable<string> skills)
        {
            var userExists = await _userRepository.GetByUserNameAsync(user.UserName);
            if (userExists != null)
            {
                return new ResultModel<ApplicationUser>
                {
                    IsSucces = false,
                    Errors = new List<string> { "User already exists" }
                };
            }
            foreach (var skillName in skills)
            {
                var skill = _skillRepository.GetByName(skillName).Result;
                if (skill != null && !user.Skills.Contains(skill))
                {
                    user.Skills.Add(skill);
                }
            }

            return new ResultModel<ApplicationUser>
            {
                IsSucces = true,
                Value = user
            };
        }

        public async Task<ResultModel<ApplicationUser>> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                return new ResultModel<ApplicationUser>
                {
                    IsSucces = true,
                    Value = user
                };
            }
            return new ResultModel<ApplicationUser>
            {
                IsSucces = false,
                Errors = new List<string> { "User not found" }
            };
        }
    }
}
