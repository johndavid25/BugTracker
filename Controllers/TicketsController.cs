using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Data.Enums;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly SignInManager<BTUser> _signInManager;
        private readonly IBTHistoryService _historyService;
        private readonly IBTProjectService _projectService;
        private readonly IBTRoleService _roleService;

        public TicketsController(
            ApplicationDbContext context,
            UserManager<BTUser> userManager,
            SignInManager<BTUser> signInManager,
            IBTHistoryService historyService,
            IBTProjectService projectService,
            IBTRoleService roleService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _historyService = historyService;
            _projectService = projectService;
            _roleService = roleService;
        }

        public IActionResult ShowFile(int id)
        {
            TicketAttachment ticketAttachment = _context.TicketAttachments.Find(id);
            string fileName = ticketAttachment.FileName;
            byte[] fileData = ticketAttachment.FileData;
            string ext = Path.GetExtension(fileName).Replace(".", "");

            Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");
            return File(fileData, $"application/{ext}");
        }

        public async Task<IActionResult> AcceptInvite(string userId, string code)
        {
            var realGuid = Guid.Parse(code);
            var invite = _context.Invites.FirstOrDefault(i => i.CompanyToken == realGuid && i.InviteeId == userId);
            if (invite is null)
            {
                return NotFound();
            }
            if (invite.IsValid)
            {
                invite.IsValid = false;
                var user = await _context.Users.FindAsync(userId);
                await _signInManager.SignInAsync(user, isPersistent: false);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return NotFound();
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketType)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .ToListAsync();
            return View(await applicationDbContext);
        }

        // GET: Tickets (My Tickets) 
        public async Task<IActionResult> MyTickets()
        {
            var model = new List<Ticket>();
            if (User.IsInRole(Roles.Admin.ToString()))//Test if Admin
            {
                model = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketType)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .ToListAsync();
            }
            else if (User.IsInRole(Roles.ProjectManager.ToString()))//Test if Project Manager
            {
                var userId = _userManager.GetUserId(User);
                var projects = await _projectService.ListUserProjectsAsync(userId);

                model = projects.SelectMany(p => p.Tickets).ToList();
            }
            else if (User.IsInRole(Roles.Developer.ToString()))//Test if Developer
            {
                var userId = _userManager.GetUserId(User);
                model = _context.Tickets.Where(t => t.DeveloperUserId == userId).ToList();
            }
            else //Test if Owner
            {
                var userId = _userManager.GetUserId(User);
                model = _context.Tickets.Where(t => t.OwnerUserId == userId).ToList();
            }


            return View(model);
        }

        public async Task<IActionResult> GoToTicket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync((int)id);

            if (notification == null)
            {
                return NotFound();
            }

            notification.Viewed = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = notification.TicketId });
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketType)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.Comments)
                .ThenInclude(t => t.User)
                .Include(t => t.Histories)
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = ("Admin, ProjectManager, Developer, Submitter"))]
        public IActionResult Create()
        {
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatus, "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Admin, ProjectManager, Developer, Submitter"))]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,OwnerUserId,DeveloperUserId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.Now;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,OwnerUserId,DeveloperUserId,Title,Description,")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            Ticket oldTicket = await _context.Tickets
                .Include(t => t.TicketType)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.Project)
                .Include(t => t.DeveloperUser)
                .AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();

                    //Add History
                    //Get User Id 
                    string userId = _userManager.GetUserId(User);

                    //Get New Ticket
                    Ticket newTicket = await _context.Tickets
                        .Include(t => t.TicketType)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.Project)
                        .Include(t => t.DeveloperUser)
                        .AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

                    //Call History Service
                    await _historyService.AddHistoryAsync(oldTicket, newTicket, userId);

                    ticket.Updated = DateTimeOffset.Now;

                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketType)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
