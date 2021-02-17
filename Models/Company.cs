using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Company
    {
        //Keys
        public int Id { get; set; }

        //Description
        public string Name { get; set; }
        public string Description { get; set; }

        //Navigation
        public virtual ICollection<BTUser> Collaborators { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
