using SmartTour.Business.Funct;
using SmartTour.Domain;
using System.Linq;

namespace SmartTour.Business
{
    public class FeedService : IFeedService
    {
        private readonly IAddPost _addPost;
        private readonly IGetPosts _getPosts;

        public FeedService(IAddPost addPost, IGetPosts getPosts)
        {
            _addPost = addPost;
            _getPosts = getPosts;
        }

        public PostEntity AddPost(PostEntity post)
        {
            return _addPost.Add(post);
        }

        public IQueryable<PostEntity> GetPosts(int uid)
        {
            return _getPosts.Get(uid);
        }
    }
}
