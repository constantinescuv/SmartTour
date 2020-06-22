using Microsoft.AspNetCore.Mvc;
using SmartTour.Domain;
using SmartTour.Business;

namespace SmartTour.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthEntity user)
        {
            try 
            {
                bool res = _authService.Register(user);
                if (res == false)
                {
                    return BadRequest("Email already in use");
                }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
           
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginEntity user)
        {
            try
            {
                (AuthEntity, bool) res = _authService.Login(user);
                if (res.Item2 == false)
                {
                    return NotFound();
                }
                else return Ok(res.Item1);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPatch("edit")]
        public IActionResult Edit([FromBody] EditEntity user)
        {
            try
            {
                (AuthEntity, bool) res = _authService.Edit(user);
                if (res.Item2 == false)
                {
                    return NotFound();
                }
                else return Ok(res.Item1);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPatch("incrementTours")]
        public IActionResult IncrementTours([FromBody] AuthEntity user)
        {
            try
            {
                bool res = _authService.IncrementTours(user);
                if (res == false)
                {
                    return NotFound();
                }
                else return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPatch("incrementPlaces")]
        public IActionResult IncrementPlaces([FromBody] AuthEntity user)
        {
            try
            {
                bool res = _authService.IncrementPlaces(user);
                if (res == false)
                {
                    return NotFound();
                }
                else return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpGet("refreshProfile")]
        public IActionResult RefreshProfile(int uid)
        {
            try
            {
                AuthEntity res = _authService.Refresh(uid);
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }

        }

    }
}
