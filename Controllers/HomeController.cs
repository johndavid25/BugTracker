using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Models.ViewModels;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBTRoleService _roleService;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IBTRoleService roleService, ApplicationDbContext context)
        {
            _logger = logger;
            _roleService = roleService;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            DashboardViewModel model = new DashboardViewModel();
            var tickets = _context.Tickets
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketPriority)
                .ToList();

            var projects = _context.Projects
                .Include(p => p.Company)
                .Include(p => p.Members)
                .ToList();

            model.Tickets = tickets;
            model.Projects = projects;

            return View(model);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AdminDashboard(List<string> userIds, string roleName)
        {
            //Go through all of the userIds I sent one at a time
            foreach (var userId in userIds)
            {
                //Us the userId to find the whole user record 
                BTUser user = await _context.Users.FindAsync(userId);
                //Make sure the user is not already in the role chosen
                if (!await _roleService.IsUserInRoleAsync(user, roleName))
                {
                    //Find all the roles the user currently occupies 
                    var userRoles = await _roleService.ListUserRolesAsync(user);
                    //Go through them one at a time
                    foreach (var role in userRoles)
                    {
                        //Remove the user from any and all roles they occupy
                        await _roleService.RemoveUserFromRoleAsync(user, role);
                    }
                    //Add the user to the chosen role
                    await _roleService.AddUserToRoleAsync(user, roleName);
                }
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageRoles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageRoles(List<string> userIds, string roleName)
        {
            //Go through all of the userIds I sent one at a time
            foreach (var userId in userIds)
            {
                //Us the userId to find the whole user record 
                BTUser user = await _context.Users.FindAsync(userId);
                //Make sure the user is not already in the role chosen
                if (!await _roleService.IsUserInRoleAsync(user, roleName))
                {
                    //Find all the roles the user currently occupies 
                    var userRoles = await _roleService.ListUserRolesAsync(user);
                    //Go through them one at a time
                    foreach (var role in userRoles)
                    {
                        //Remove the user from any and all roles they occupy
                        await _roleService.RemoveUserFromRoleAsync(user, role);
                    }
                    //Add the user to the chosen role
                    await _roleService.AddUserToRoleAsync(user, roleName);
                }
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
