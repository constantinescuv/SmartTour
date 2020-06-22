using SmartTour.Domain;

namespace SmartTour.Business.Funct
{
    public interface ILogin
    {
        public (AuthEntity, bool) LoginAccount(LoginEntity user);
    }
}
