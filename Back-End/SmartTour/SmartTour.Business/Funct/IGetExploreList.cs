using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTour.Business.Funct
{
    public interface IGetExploreList
    {
        public (List<AttractionEntity>, List<RestaurantEntity>) ReturnList(CoordinateEntity coordinates);
    }
}
