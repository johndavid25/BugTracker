using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ViewModels
{
    public class DashboardViewModel
    {
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        //public int PercentStatusNew { get; set; }
        //public int PercentStausOpen { get; set; }
        //public int PercentStatusDev { get; set; }
        //public int PercentStatusClosed { get; set; }
    }
}
