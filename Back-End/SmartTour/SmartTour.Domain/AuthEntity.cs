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
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Image { get; set; }

        public decimal ToursCompleted { get; set; }

        public decimal PlacesVisited { get; set; }

        [Required]
        public string Passw { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
