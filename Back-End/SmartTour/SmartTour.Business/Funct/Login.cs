using SmartTour.DataAccess;
using SmartTour.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SmartTour.Business.Funct
{
    public class Login : ILogin
    {
        private readonly UsersRepository _auc;
        public Login(UsersRepository auc)
        {
            _auc = auc;
        }

        public (AuthEntity, bool) LoginAccount(LoginEntity user)
        {
            var dbEntry = _auc.Users.FirstOrDefault(acc => acc.Email == user.Email);
            using (HashAlgorithm alg = SHA256.Create())
            {
                string password = Encoding.UTF8.GetString(alg.ComputeHash(Encoding.UTF8.GetBytes(user.Passw)));
                user.Passw = password;

            }
            if (dbEntry.Passw == user.Passw)
            {
                return (dbEntry, true);
            }

            else return (new AuthEntity(), false);
        }
    }
}
