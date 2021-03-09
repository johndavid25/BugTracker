using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTNotificationsService
    {
        //When we talk about a "user" there are two verison of a user 
        //A reacord in the User table - represents any person who is registered
        //The other is capital U User - this is the currently logged in user ClaimsPrinciple User

        public Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(ClaimsPrincipal currentUser);
    }
}
