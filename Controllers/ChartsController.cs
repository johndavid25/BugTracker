using BugTracker.Data;
using BugTracker.Models.ChartModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly List<string> _backgroundColors;
        private readonly List<string> _borderColors;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
            _backgroundColors = new List<string>
            {
                //"#FF0000",
                //"#19FF5A",
                //"#FFCE19",
                //"#0D22FF",
                //"#FE7B00",
                //"#19D0FF",
                //"#D20DFF",
                //"#A0FF19",
                //"#00FF06",
                //"#000000"

                "rgba(255, 99, 132, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)",
                "rgba(153, 102, 255, 0.2)",
                "rgba(255, 159, 64, 0.2)",
                "rgba(210, 13, 255, 0.2)",
                "rgba(160, 255, 25, 0.2)",
                "rgba(255, 25, 208, 0.2)",
                "rgba(255, 255, 255, 0.2)"

            };
            _borderColors = new List<string>
            {
                "rgba(255, 99, 132, 1)",
                "rgba(54, 162, 235, 1)",
                "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)",
                "rgba(153, 102, 255, 1)",
                "rgba(255, 159, 64, 1)",
                "rgba(210, 13, 255, 1)",
                "rgba(160, 255, 25, 1)",
                "rgba(255, 25, 208, 1)",
                "rgba(255, 255, 255, 1)"
            };
        }

        public JsonResult PriorityChart()
        {
            var result = new ChartJSModel();
            var priorities = _context.TicketPriorities.ToList();
            int count = 0;
            foreach (var priority in priorities)
            {
                result.Labels.Add(priority.Name);
                result.Data.Add(_context.Tickets.Where(t => t.TicketPriorityId == priority.Id).Count());
                if (count < 10)
                {
                    result.BackgroundColors.Add(_backgroundColors[count]);
                    result.BorderColors.Add(_borderColors[count]);
                }
                else
                {
                    result.BackgroundColors.Add(_backgroundColors[count % 10]);
                    result.BorderColors.Add(_borderColors[count % 10]);
                }
                count++;
            }
            return Json(result);
        }
        
        public JsonResult TypeChart()
        {
            var result = new ChartJSModel();
            var types = _context.TicketTypes.ToList();
            int count = 0;
            foreach (var type in types)
            {
                result.Labels.Add(type.Name);
                result.Data.Add(_context.Tickets.Where(t => t.TicketPriorityId == type.Id).Count());
                if (count < 10)
                {
                    result.BackgroundColors.Add(_backgroundColors[count]);
                    result.BorderColors.Add(_borderColors[count]);
                }
                else
                {
                    result.BackgroundColors.Add(_backgroundColors[count % 10]);
                    result.BorderColors.Add(_borderColors[count % 10]);
                }
                count++;
            }
            return Json(result);
        }

        public JsonResult StatusChart()
        {
            var result = new ChartJSModel();
            var statuses = _context.TicketStatus.ToList();
            int count = 0;
            foreach (var status in statuses)
            {
                result.Labels.Add(status.Name);
                result.Data.Add(_context.Tickets.Where(t => t.TicketPriorityId == status.Id).Count());
                if (count < 10)
                {
                    result.BackgroundColors.Add(_backgroundColors[count]);
                    result.BorderColors.Add(_borderColors[count]);
                }
                else
                {
                    result.BackgroundColors.Add(_backgroundColors[count % 10]);
                    result.BorderColors.Add(_borderColors[count % 10]);
                }
                count++;
            }
            return Json(result);
        }
    }
}
