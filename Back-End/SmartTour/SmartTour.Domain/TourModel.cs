using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTour.Domain
{
    public class TourModel
    {
        public IEnumerable<PlaceEntity> Tour { set; get; }

        public TourModel(IEnumerable<PlaceEntity> tour)
        {
            this.Tour = tour;
        }

        public List<string> GetPlacesByName() 
        {
            List<string> Names = new List<string>();
            foreach (PlaceEntity Place in Tour)
            {
                Names.Add(Place.Name);
            }
            return Names;
        }
    }
}
