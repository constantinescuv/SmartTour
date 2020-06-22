using Microsoft.AspNetCore.Mvc;
using SmartTour.Business;
using SmartTour.Domain;
using System.Collections.Generic;

namespace SmartTour.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExploreController : ControllerBase
    {
        private readonly IExploreService _exploreService;

        public ExploreController(IExploreService exploreService)
        {
            _exploreService = exploreService;
        }

        [HttpGet("getExploreList")]
        public IActionResult GetExploreList(string Latitude, string Longitude)
        {
            try
            {
                CoordinateEntity coordinates = new CoordinateEntity();
                coordinates.Latitude = Latitude;
                coordinates.Longitude = Longitude;
                (List<AttractionEntity>, List<RestaurantEntity>) res = _exploreService.GetExploreList(coordinates);

                return Ok(new Dictionary<string, dynamic> { { "attractions", res.Item1 },
                                                            { "restaurants", res.Item2 } });
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}