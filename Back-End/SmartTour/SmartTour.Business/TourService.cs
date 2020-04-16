using SmartTour.Business.Funct;
using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.Business
{
    public class TourService : ITourService
    {
        private readonly IGetTour _getTour;

        public TourService(IGetTour getTour)
        {
            _getTour = getTour;
        }

        public Task<TourModel> GetTour(TourDetailsEntity tourDetails) 
        {
            return _getTour.ReturnTourBasedOnCriteria(tourDetails);
        }

    }
}
