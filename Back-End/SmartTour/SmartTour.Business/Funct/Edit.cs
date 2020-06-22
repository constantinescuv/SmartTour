using SmartTour.DataAccess;
using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartTour.Business.Funct
{
    public class Edit : IEdit
    {
        private readonly UsersRepository _auc;
        public Edit(UsersRepository auc)
        {
            _auc = auc;
        }

        public (AuthEntity, bool) EditAccount(EditEntity user)
        {
            var dbEntry = _auc.Users.FirstOrDefault(acc => acc.Email == user.Email);

            if (dbEntry != null)
            {
                if (user.FirstName != string.Empty) { dbEntry.FirstName = user.FirstName; }
                if (user.LastName != string.Empty) { dbEntry.LastName = user.LastName; }
                if (user.Image != string.Empty) { dbEntry.Image = user.Image; }
                if (user.ResetTours != 0) { dbEntry.ToursCompleted = 0; }
                if (user.ResetPlaces != 0) { dbEntry.PlacesVisited = 0; }
                _auc.SaveChanges();
                return (dbEntry, true);
            }
            else return (new AuthEntity(), false);
        }
    }
}
