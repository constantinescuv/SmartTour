using SmartTour.DataAccess;
using SmartTour.Domain;
using System;

namespace SmartTour.Business.Funct
{
    public class AddPost : IAddPost
    {
        private readonly PostsRepository _posts;

        public AddPost(PostsRepository posts)
        {
            _posts = posts;
        }
        
        public PostEntity Add(PostEntity post)
        {
            post.dt_created = DateTime.Now;
            _posts.Add(post);
            _posts.SaveChanges();
            return post;
        }
    }
}
