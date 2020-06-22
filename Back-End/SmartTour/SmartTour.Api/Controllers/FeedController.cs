using Microsoft.AspNetCore.Mvc;
using SmartTour.Business;
using SmartTour.Domain;

namespace SmartTour.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class FeedController : ControllerBase
    {
        private readonly IFeedService _feedService;

        public FeedController(IFeedService feedService)
        {
            _feedService = feedService;
        }

        [HttpPost("addPost")]
        public IActionResult AddPost([FromBody] PostEntity post)
        {
            try
            {
                _feedService.AddPost(post);
                return Created("addPost", post);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpGet("getPosts")]
        public IActionResult GetPosts(int uid)
        {
            try
            {
                return Ok(_feedService.GetPosts(uid));
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}