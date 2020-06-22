using SmartTour.Domain;

namespace SmartTour.Business.Funct
{
    public interface IRefreshProfile
    {
        public AuthEntity Refresh(int uid);
    }
}