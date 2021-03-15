using BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTInviteService
    {
        public Task<string> InviteWizardAsync(InviteViewModel inviteViewModel);
    }
}
