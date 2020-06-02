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

        [HttpPost("getTour")]
        public IActionResult GetTour([FromBody] TourDetailsEntity tourDetails)
        {
            try
            {
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
                    

                return Ok(new Dictionary<string, dynamic> { { "tour", attractionEntities },
                                                            { "restaurants", restaurantEntities },
                                                            { "restaurantPosition", restaurantPosition},
                                                            { "backup", res.Item2 } });
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}