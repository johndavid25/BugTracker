using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ViewModels
{
    public class InviteViewModel
    {
        //public BTUser User { get; set; }
        //Need Email, FirstName, LastName

        //public Company Company { get; set; }
        //Need Name and Description 

        //public Project Project { get; set; }
        //Need Name and Description 

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Company Description")]
        public string CompanyDescription { get; set; }
    }
}
