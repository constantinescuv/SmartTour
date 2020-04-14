using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartTour.Domain
{
    public class EditEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Image { get; set; }

        public int ResetTours { get; set; }

        public int ResetPlaces { get; set; }

        public string Passw { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
