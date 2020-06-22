using SmartTour.DataAccess;
using SmartTour.Domain;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SmartTour.Business.Funct
{
    public class Register : IRegister
    {
        private readonly UsersRepository _auc;
        public Register(UsersRepository auc)
        {
            _auc = auc;
        }
        public bool RegisterAccount(AuthEntity user)
        {
            var dbEntry = _auc.Users.FirstOrDefault(acc => acc.Email == user.Email);
            if (dbEntry != null)
                return false;

            using (HashAlgorithm alg = SHA256.Create())
            {
                string password = Encoding.UTF8.GetString(alg.ComputeHash(Encoding.UTF8.GetBytes(user.Passw)));
                user.Passw = password;

            }
            user.Image = "https://moonvillageassociation.org/wp-content/uploads/2018/06/default-profile-picture1.jpg";
            user.PlacesVisited = 0;
            user.ToursCompleted = 0;
            _auc.Users.Add(user);
            _auc.SaveChanges();
            return true;
        }
    }
}
