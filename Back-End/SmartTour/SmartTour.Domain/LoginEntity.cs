﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartTour.Domain
{
    public class LoginEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Image { get; set; }

        public int ToursCompleted { get; set; }

        public int PlacesVisited { get; set; }

        public string Passw { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
