using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ManageRoles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(List<string> userIds, string roleName)
        {
            //Go through alll of the userIds I sent one at a time
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
                    foreach(var role in userRoles)
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
