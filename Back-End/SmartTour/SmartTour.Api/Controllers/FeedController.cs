using Microsoft.AspNetCore.Mvc;
using SmartTour.DataAccess;
using SmartTour.Domain;
using System;
using System.Linq;

namespace SmartTour.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class FeedController : ControllerBase
    {
        private readonly DatabaseContext _auc;
        public FeedController(DatabaseContext auc)
        {
            _auc = auc;
        }

        [HttpPost("addPost")]
        public IActionResult AddPost([FromBody] PostEntity post)
        {
            try
            {
                post.dt_created = DateTime.Now;
                _auc.Add(post);
                _auc.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }

            return Created("addPost", post);
        }

        [HttpPost("getPosts")]
        public IActionResult GetPosts([FromBody] AuthEntity user)
        {
            try
            {
                var dbEntry = _auc.Posts.Where(acc => acc.UserId == user.UserId);
                
                return Ok(dbEntry);
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}