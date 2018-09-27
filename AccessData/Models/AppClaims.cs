using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccessData.Models
{
    public class AppClaims
    {
        [Key]
        public int Id { set; get; }

        [Required]
        public string Name { set; get; }

        public string Description { set; get; }

        [Required]
        public string Value { set; get; }
    }
}
