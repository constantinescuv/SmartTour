using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartTour.Domain
{
    public class AuthEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Passw { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
