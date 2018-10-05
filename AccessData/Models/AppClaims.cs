using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccessData.Models
{
    public class AppClaims
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        public string Name { set; get; }

        public string Description { set; get; }

        [Required]
        public string Value { set; get; }

        [ForeignKey("Aplication")]
        public int ApplicationId { set; get; }

        public virtual Aplication Aplication { set; get; }
    }
}
