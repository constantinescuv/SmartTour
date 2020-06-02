using SmartTour.Business.Funct;
using SmartTour.Domain;
using System.Collections.Generic;

namespace SmartTour.Business
{
    public class TourService : ITourService
    {
        private readonly IGetTour _getTour;

        public TourService(IGetTour getTour)
        {
            _getTour = getTour;
        }

        public (TourModel, List<List<PlaceEntity>>) GetTour(TourDetailsEntity tourDetails) 
        {
            return _getTour.ReturnTourBasedOnCriteria(tourDetails);
        }

    }
}
