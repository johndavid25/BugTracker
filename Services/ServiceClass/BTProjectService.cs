﻿using BugTracker.Data;
using BugTracker.Data.Enums;
using BugTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBTRoleService _roleService;

        public BTProjectService(
            ApplicationDbContext context,
            UserManager<BTUser> userManager,
            IHttpContextAccessor contextAccessor,
            IBTRoleService roleService)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _roleService = roleService;
        }
        public async Task AddUserToProjectAsync(string userId, int projectId)
        {
            //Needs to check for Project Manager - only one allowed
            try
            {
                if(!await IsUserOnProjectAsync(userId, projectId))
                {

                    BTUser user = await _userManager.FindByIdAsync(userId);
                    if (await _roleService.IsUserInRoleAsync(user, Roles.ProjectManager.ToString()))
                    {
                        var oldManager = await ProjectManagerOnProjectsAsync(projectId);
                        if(oldManager != null)
                        {
                            await RemoveUserFromProjectAsync(oldManager.Id, projectId);
                        }
                    }
                    Project project = await _context.Projects.FindAsync(projectId);

                    try
                    {
                        project.Members.Add(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"****ERROR**** - Error Adding user to project --> { ex.Message}");
            }
        }

        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            var project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == projectId);
            var flag = project.Members.Any(u => u.Id == userId);
            return flag;
        }

        public async Task<IEnumerable<Project>> ListUserProjectsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (await _roleService.IsUserInRoleAsync(user, Roles.Admin.ToString()))
            {
                return await _context.Projects.ToListAsync();
            }         
            var output = new List<Project>();
            foreach(var project in await _context.Projects
                .Include(t => t.Tickets)
                .ThenInclude(t => t.TicketPriority)
                .Include(t => t.Tickets)
                .ThenInclude(t => t.TicketStatus)
                .Include(t => t.Tickets)
                .ThenInclude(t => t.TicketType)
                .ToListAsync())
            {
                if(await IsUserOnProjectAsync(userId, project.Id))
                {
                    output.Add(project);
                }
            }
            return output;
        }

        public async Task<BTUser> ProjectManagerOnProjectsAsync(int projectId)
        {
            var projectManagers = await _userManager.GetUsersInRoleAsync(Roles.ProjectManager.ToString());
            var onProject = await UsersOnProjectAsync(projectId);
            var projectManager = projectManagers.Intersect(onProject).FirstOrDefault();
            return projectManager;
        }

        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                if (await IsUserOnProjectAsync(userId, projectId))
                {
                    BTUser user = await _userManager.FindByIdAsync(userId);
                    Project project = await _context.Projects.FindAsync(projectId);

                    try
                    {
                        project.Members.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"****ERROR**** - Error Removing user to project --> { ex.Message}");
            }
        }

        public async Task<IEnumerable<BTUser>> DevelopersOnProjectsAsync(int projectId)
        {
            var developers = await _userManager.GetUsersInRoleAsync(Roles.Developer.ToString());
            var onProject = await UsersOnProjectAsync(projectId);
            var devsOnProject = developers.Intersect(onProject).ToList();
            return devsOnProject; 
        }

        public async Task<IEnumerable<BTUser>> SubmittersOnProjectsAsync(int projectId)
        {
            var submitters = await _userManager.GetUsersInRoleAsync(Roles.Submitter.ToString());
            var onProject = await UsersOnProjectAsync(projectId);
            var subsOnProject = submitters.Intersect(onProject).ToList();
            return subsOnProject;
        }

        public async Task<IEnumerable<BTUser>> UsersNotOnProjectAsync(int projectId)
        {
            var output = new List<BTUser>();
            foreach (var user in await _context.Users.ToListAsync())
            {
                if (!await IsUserOnProjectAsync(user.Id, projectId))
                {
                    output.Add(user);
                }
            }
            return output;
        }

        public async Task<IEnumerable<BTUser>> UsersOnProjectAsync(int projectId)
        {
            var output = new List<BTUser>();
            foreach (var user in await _context.Users.ToListAsync())
            {
                if(await IsUserOnProjectAsync(user.Id, projectId))
                {
                    output.Add(user);
                }
            }
            return output;
        }
    }
}
