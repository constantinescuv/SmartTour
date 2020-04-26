using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartTour.Domain
{
    public class PlaceEntity
    {
        [Required]
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Photo { get; set; }
        public List<Dictionary<string,string>> Subtype { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Rating { get; set; }
        public string Website { get; set; }
    }
}
