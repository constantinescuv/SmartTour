using SmartTour.Domain;
using System.Linq;

namespace SmartTour.Business
{
    public interface IFeedService
    {
        public PostEntity AddPost(PostEntity post);
        public IQueryable<PostEntity> GetPosts(int uid);
    }
}
