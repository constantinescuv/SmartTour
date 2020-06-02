using SmartTour.Business.Funct;
using SmartTour.Domain;
using System.Collections.Generic;

namespace SmartTour.Business
{
    public class ExploreService : IExploreService
    {
        private readonly IGetExploreList _getExploreList;

        public ExploreService(IGetExploreList getExploreList)
        {
            _getExploreList = getExploreList;
        }

        public (List<AttractionEntity>, List<RestaurantEntity>) GetExploreList(CoordinateEntity coordinates)
        {
            return _getExploreList.ReturnList(coordinates);
        }

    }
}
