using SmartTour.DataAccess;
using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartTour.Business.Funct
{
    public class GetPosts : IGetPosts
    {
        private readonly PostsRepository _posts;

        public GetPosts(PostsRepository posts)
        { 
            _posts = posts;
        }

        public IQueryable<PostEntity> Get(int uid)
        {
            var dbEntry = _posts.Posts.Where(acc => acc.UserId == uid);
            return dbEntry;
        }
    }
}
