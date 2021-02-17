using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class TicketCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketComments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketComments.Include(t => t.Ticket);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TicketComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketComments = await _context.TicketComments
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketComments == null)
            {
                return NotFound();
            }

            return View(ticketComments);
        }

        // GET: TicketComments/Create
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketId,UserId,Comment,Created")] TicketComments ticketComments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketComments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComments.TicketId);
            return View(ticketComments);
        }

        // GET: TicketComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketComments = await _context.TicketComments.FindAsync(id);
            if (ticketComments == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComments.TicketId);
            return View(ticketComments);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketId,UserId,Comment,Created")] TicketComments ticketComments)
        {
            if (id != ticketComments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketComments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketCommentsExists(ticketComments.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComments.TicketId);
            return View(ticketComments);
        }

        // GET: TicketComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketComments = await _context.TicketComments
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketComments == null)
            {
                return NotFound();
            }

            return View(ticketComments);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketComments = await _context.TicketComments.FindAsync(id);
            _context.TicketComments.Remove(ticketComments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketCommentsExists(int id)
        {
            return _context.TicketComments.Any(e => e.Id == id);
        }
    }
}
