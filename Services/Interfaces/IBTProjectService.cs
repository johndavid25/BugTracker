using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public interface IBTProjectService
    {
        //Is the user on a project
        public Task<bool> IsUserOnProjectAsync(string userId, int projectId);

        //All users on the project 
        public Task<IEnumerable<BTUser>> UsersOnProjectAsync(int projectId);

        //All users not on the project
        public Task<IEnumerable<BTUser>> UsersNotOnProjectAsync(int projectId);

        //Assign/Add user to a project 
        public Task AddUserToProjectAsync(string userId, int projectId);

        //Remove from project
        public Task RemoveUserFromProjectAsync(string userId, int projectId);

        //All project for one user 
        public Task<IEnumerable<Project>> ListUserProjectsAsync(string userId);

        //Developers on projects
        public Task<IEnumerable<BTUser>> DevelopersOnProjectsAsync(int projectId);

        //Submitters on projects 
        public Task<IEnumerable<BTUser>> SubmittersOnProjectsAsync(int projectId);

        //Project Manager on projects
        public Task<BTUser> ProjectManagerOnProjectsAsync(int projectId);

    }
}
