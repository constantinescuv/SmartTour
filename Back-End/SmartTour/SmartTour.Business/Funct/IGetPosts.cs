using SmartTour.Domain;
using System.Linq;

namespace SmartTour.Business.Funct
{
    public interface IGetPosts
    {
        public IQueryable<PostEntity> Get(int uid);
    }
}
