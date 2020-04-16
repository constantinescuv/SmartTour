using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartTour.Domain
{
    public class TourDetailsEntity
    {
        [Required]
        public string Transport { get; set; }

        [Required]
        public string TimeRange { get; set; }

        [Required]
        public string DistanceRange { get; set; }

        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }

    }
}
