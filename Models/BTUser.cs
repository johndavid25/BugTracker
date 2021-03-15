using BugTracker.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class BTUser : IdentityUser
    {
        //Keys 
        public int CompanyId { get; set; }

        //Description
        [Required]
        [StringLength(50)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Select Image")]
        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize(2 * 1024 * 1024)]
        [AllowExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile AvatarFormFile { get; set; }

        public string AvatarFileName { get; set; }
        public byte[] AvatarFileData { get; set; }

        //Navigation        
        public virtual Company Company { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        //Other
        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}
