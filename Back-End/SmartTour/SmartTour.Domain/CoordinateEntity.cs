using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartTour.Domain
{
    public class CoordinateEntity
    {
        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }
    }
}
