using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Repository.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(15)]
        public string Document { get; set; }

        [Required]
        public string Names { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
