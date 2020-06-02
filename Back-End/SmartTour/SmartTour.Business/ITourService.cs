using SmartTour.Domain;
using System.Collections.Generic;

namespace SmartTour.Business
{
    public interface ITourService
    {
        (TourModel, List<List<PlaceEntity>>) GetTour(TourDetailsEntity tourDetails);
    }
}
