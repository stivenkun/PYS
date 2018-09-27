using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccessData.Models
{
    public class Aplication
    {
        [Key]
        public int Id { set; get; }

        [Required]
        public string Name { set; get; }

        [Url]
        [Required]
        public string Url { set; get; }
 
        public string Description { set; get; }

        public bool IsActive { set; get; }

        public string IconSrc { set; get; }

        public virtual List<AppClaims> AppClaims { set; get; }
    }
}
