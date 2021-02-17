﻿using BugTracker.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class BTUser : IdentityUser
    {
        //Description
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
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
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        //Other
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}