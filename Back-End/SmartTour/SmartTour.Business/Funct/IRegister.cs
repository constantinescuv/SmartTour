using SmartTour.Domain;

namespace SmartTour.Business.Funct
{
    public interface IRegister
    {
        bool RegisterAccount(AuthEntity user);
    }
}
