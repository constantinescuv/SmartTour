using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTour.DataAccess;
using SmartTour.Domain;
using System.Security.Cryptography;
using System.Text;

namespace SmartTour.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _auc;
        public AuthController(DatabaseContext auc)
        {
            _auc = auc;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthEntity user)
        {
            try 
            {
                using (HashAlgorithm alg = SHA256.Create())
                {
                    string password = Encoding.UTF8.GetString(alg.ComputeHash(Encoding.UTF8.GetBytes(user.Passw)));
                    user.Passw = password;

                }
                _auc.Add(user);
                _auc.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
           
            return Created("register", user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginEntity user)
        {
            try
            {
                var dbEntry = _auc.Users.FirstOrDefault(acc => acc.Email == user.Email);
                using (HashAlgorithm alg = SHA256.Create())
                {
                    string password = Encoding.UTF8.GetString(alg.ComputeHash(Encoding.UTF8.GetBytes(user.Passw)));
                    user.Passw = password;

                }
                if (dbEntry.Passw == user.Passw)
                {
                    return Ok();
                }

                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }

        }

    }
}
