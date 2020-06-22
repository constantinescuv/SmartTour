using SmartTour.Domain;

namespace SmartTour.Business.Funct
{
    public interface IIncrement
    {
        public bool IncrementTours(AuthEntity user);
        public bool IncrementPlaces(AuthEntity user);

    }
}