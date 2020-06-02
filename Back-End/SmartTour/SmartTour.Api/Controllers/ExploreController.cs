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

        [HttpPost("getExploreList")]
        public IActionResult GetExploreList([FromBody] CoordinateEntity coordinates)
        {
            try
            {
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