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
                user.Image = "https://moonvillageassociation.org/wp-content/uploads/2018/06/default-profile-picture1.jpg";
                user.PlacesVisited = 0;
                user.ToursCompleted = 0;
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
                    return Ok(dbEntry);
                }

                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPost("edit")]
        public IActionResult Edit([FromBody] EditEntity user)
        {
            try
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
                }
                return Ok(dbEntry);
                
            }
            catch
            {
                return BadRequest();
            }

        }

    }
}
