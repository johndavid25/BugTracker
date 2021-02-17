using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Notification
    {
        //Keys 
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string RecipientId { get; set; }
        public string SenderId { get; set; }

        //Description
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool Viewed { get; set; }

        //Navigation
        public virtual Ticket Ticket { get; set; }
        public virtual BTUser Recipient { get; set; }
        public virtual BTUser Sender { get; set; }
    }
}
