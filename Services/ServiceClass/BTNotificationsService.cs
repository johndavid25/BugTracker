using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BugTracker.Services.ServiceClass
{
    public class BTNotificationsService : IBTNotificationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;

        //I need access to the database to get the notifications
        //I need the UserManager to covert ClaimsPrincipal to BTUser

        public BTNotificationsService(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(ClaimsPrincipal currentUser)
        {
            //Convert the ClaimsPrincipal into a BTUser - database doesn't know ClaimsPrincipal 
            BTUser user = await _userManager.GetUserAsync(currentUser);

            var userNotifications = _context.Notifications.Where(n => n.RecipientId == user.Id).Include(n => n.Sender);
            var unreadNotifications = await userNotifications.Where(n => n.Viewed == false).ToListAsync();

            //var unreadNotifications = _context.Notfications.Where(n = > n.RrecipientId == user.Id && !n.Viewed).Include(n => n.Sender).ToListAsync();

            return unreadNotifications;
        }
    }
}
