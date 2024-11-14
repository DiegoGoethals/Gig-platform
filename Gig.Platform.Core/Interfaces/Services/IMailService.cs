using Gig.Platform.Core.Entities;
using Gig.Platform.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gig.Platform.Core.Interfaces.Services
{
    public interface IMailService
    {
        Task<ResultModel<bool>> SendValidationEmail(ApplicationUser user, string link, string sender, string apikey);
    }
}
