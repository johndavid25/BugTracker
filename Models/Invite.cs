using System;

namespace BugTracker.Models
{
    public class Invite
    {
        //Keys 
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string InvitorId { get; set; }
        public string InviteeId { get; set; }

        //Desription
        public string Email { get; set; }
        public Guid CompanyToken { get; set; }
        public DateTimeOffset InviteDate { get; set; }
        public bool IsValid { get; set; }

        //Navigation
        public virtual Company Company { get; set; }
        public virtual BTUser Invitor { get; set; }
        public virtual BTUser Invitee { get; set; }
    }
}
