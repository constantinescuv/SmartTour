using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTour.Business;
using SmartTour.Domain;

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
                var res = _tourService.GetTour(tourDetails);
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}