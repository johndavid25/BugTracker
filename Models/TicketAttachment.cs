using BugTracker.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TicketAttachment
    {
        //Keys 
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }

        //Description
        [Display(Name ="Select Image")]
        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize(2 * 1024 * 1024)]
        [AllowExtensions(new string[] { ".jpg", ".png", ".doc", "docx", ".xls", ".xlsx", ".pdf"})]
        public IFormFile Formfile { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }

        //Navigation
        public virtual Ticket Ticket { get; set; }
        public virtual BTUser User { get; set; }
    }
}
