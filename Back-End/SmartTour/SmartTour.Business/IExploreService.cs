using SmartTour.Domain;
using System.Collections.Generic;

namespace SmartTour.Business
{
    public interface IExploreService
    {
        (List<AttractionEntity>, List<RestaurantEntity>) GetExploreList(CoordinateEntity coordinates);
    }

}
