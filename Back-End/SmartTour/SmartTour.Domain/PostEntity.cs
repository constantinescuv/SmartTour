using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTour.Domain
{
    public class PostEntity
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

        public string Images { get; set; }

        public string Date { get; set; }

        public decimal Duration { get; set; }

        public string CheckpointNames { get; set; }

        public string Place { get; set; }

        public DateTime dt_created { get; set; }
    }
}
