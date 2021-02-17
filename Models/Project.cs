using BugTracker.Extensions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class Project
    {
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Members = new HashSet<BTUser>();
        }

        //Keys 
        public int Id { get; set; }

        //Description 
        [Required]
        [StringLength(50)]
        [Display(Name ="Project Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name ="Select Image")]
        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize(2 * 1024 * 1024)]
        [AllowExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile ImageFormFile { get; set; }
        public string ImageFileName { get; set; }
        public byte[] ImageFileData { get; set; }
        public int? CompanyId { get; set; }

        //Navigation
        public virtual ICollection<BTUser> Members { get; set; } 
        public virtual ICollection<Ticket> Tickets { get; set; } 
        public virtual Company Company { get; set; }
    }
}
