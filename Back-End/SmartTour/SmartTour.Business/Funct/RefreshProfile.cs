using SmartTour.DataAccess;
using SmartTour.Domain;
using System.Linq;

namespace SmartTour.Business.Funct
{
    public class RefreshProfile : IRefreshProfile
    {
        private readonly UsersRepository _auc;
        public RefreshProfile(UsersRepository auc)
        {
            _auc = auc;
        }

        public AuthEntity Refresh(int uid)
        {
            var dbEntry = _auc.Users.FirstOrDefault(acc => acc.UserId == uid);

            return dbEntry;
        }
    }
}
