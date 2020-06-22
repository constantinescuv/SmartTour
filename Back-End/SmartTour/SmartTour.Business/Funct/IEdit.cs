using SmartTour.Domain;

namespace SmartTour.Business.Funct
{
    public interface IEdit
    {
        public (AuthEntity, bool) EditAccount(EditEntity user);

    }
}