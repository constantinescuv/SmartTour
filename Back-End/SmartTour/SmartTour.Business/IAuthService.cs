using SmartTour.Domain;

namespace SmartTour.Business
{
    public interface IAuthService
    {
        bool Register(AuthEntity user);
        (AuthEntity, bool) Login(LoginEntity user);
        (AuthEntity, bool) Edit(EditEntity user);
        bool IncrementTours(AuthEntity user);
        bool IncrementPlaces(AuthEntity user);
        AuthEntity Refresh(int uid);
    }
}
