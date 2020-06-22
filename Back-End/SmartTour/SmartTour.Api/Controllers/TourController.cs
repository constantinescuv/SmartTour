using Microsoft.AspNetCore.Mvc;
using SmartTour.Business;
using SmartTour.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SmartTour.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet("getTour")]
        public IActionResult GetTour(string Latitude, string Longitude, string TimeRange, string DistanceRange, bool EatingBreak, string savedPlaces, string Transport)
        {
            try
            {
                TourDetailsEntity tourDetails = new TourDetailsEntity();
                tourDetails.Latitude = Latitude;
                tourDetails.Longitude = Longitude;
                tourDetails.savedPlaces = savedPlaces;
                tourDetails.TimeRange = TimeRange;
                tourDetails.Transport = Transport;
                tourDetails.EatingBreak = EatingBreak;
                tourDetails.DistanceRange = DistanceRange;
                (TourModel, List<List<PlaceEntity>>) res = _tourService.GetTour(tourDetails);

                List<AttractionEntity> attractionEntities = new List<AttractionEntity>();
                List<RestaurantEntity> restaurantEntities = new List<RestaurantEntity>();
                int restaurantPosition = -1;
                for (int i = 0; i < res.Item1.Tour.Count(); i++)
                {
                    var place = res.Item1.Tour.ElementAt(i);
                    if (place is AttractionEntity)
                        attractionEntities.Add((AttractionEntity)place);
                    else
                    {
                        restaurantEntities.Add((RestaurantEntity)place);
                        restaurantPosition = i;
                    }
                }

                List<List<AttractionEntity>> backupEntities = new List<List<AttractionEntity>>();
                for (int i = 0; i < res.Item2.Count(); i++)
                {
                    backupEntities.Add(new List<AttractionEntity>());
                    for (int j = 0; j < res.Item2[i].Count(); j++)
                    {
                        var place = res.Item2[i][j];
                        if (place is AttractionEntity)
                            backupEntities.ElementAt(i).Add((AttractionEntity)place);
                        else
                        {
                            backupEntities.ElementAt(i).Add((AttractionEntity)place);
                        }
                    }
                }

                return Ok(new Dictionary<string, dynamic> { { "tour", attractionEntities },
                                                            { "restaurants", restaurantEntities },
                                                            { "restaurantPosition", restaurantPosition},
                                                            { "backup", backupEntities } });
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}