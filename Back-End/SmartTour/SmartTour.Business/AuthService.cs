using SmartTour.Business.Funct;
using SmartTour.Domain;
using System.Linq;

namespace SmartTour.Business
{
    public class AuthService : IAuthService
    {
        private readonly IRegister _register;
        private readonly ILogin _login;
        private readonly IEdit _edit;
        private readonly IIncrement _increment;
        private readonly IRefreshProfile _refresh;

        public AuthService(IRegister register, ILogin login, IEdit edit, IIncrement increment, IRefreshProfile refresh)
        {
            _register = register;
            _login = login;
            _edit = edit;
            _increment = increment;
            _refresh = refresh;
        }
        public bool Register(AuthEntity user)
        {
            return _register.RegisterAccount(user);
        }
        public (AuthEntity, bool) Login(LoginEntity user)
        {
            return _login.LoginAccount(user);
        }

        public (AuthEntity, bool) Edit(EditEntity user)
        {
            return _edit.EditAccount(user);
        }

        public bool IncrementTours(AuthEntity user)
        {
            return _increment.IncrementTours(user);
        }

        public bool IncrementPlaces(AuthEntity user)
        {
            return _increment.IncrementPlaces(user);
        }

        public AuthEntity Refresh(int uid)
        {
            return _refresh.Refresh(uid);
        }
    }
}
