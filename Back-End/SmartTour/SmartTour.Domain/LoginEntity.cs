using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartTour.Domain
{
    public class LoginEntity
    {
        [Required]
        public string Passw { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
