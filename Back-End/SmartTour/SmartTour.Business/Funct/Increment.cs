using SmartTour.DataAccess;
using SmartTour.Domain;
using System.Linq;

namespace SmartTour.Business.Funct
{
    public class Increment : IIncrement
    {
        private readonly UsersRepository _auc;
        public Increment(UsersRepository auc)
        {
            _auc = auc;
        }

        public bool IncrementPlaces(AuthEntity user)
        {
            var dbEntry = _auc.Users.FirstOrDefault(acc => acc.UserId == user.UserId);

            if (dbEntry != null)
            {
                dbEntry.PlacesVisited += 1;
                _auc.SaveChanges();
                return true;
            }
            else return false;
        }

        public bool IncrementTours(AuthEntity user)
        {
            var dbEntry = _auc.Users.FirstOrDefault(acc => acc.UserId == user.UserId);

            if (dbEntry != null)
            {
                dbEntry.ToursCompleted += 1;
                _auc.SaveChanges();
                return true;
            }
            else return false;
        }
    }
}
