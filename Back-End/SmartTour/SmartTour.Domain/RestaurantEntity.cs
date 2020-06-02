using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartTour.Domain
{
    public class RestaurantEntity : PlaceEntity
    {
        public string Num_reviews { get; set; }

        public List<Dictionary<string, string>> Cuisine { get; set; }
    }
}
