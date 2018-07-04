using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Repositories;
using backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        protected PersonRepo repo;

        public PersonController()
        {
            repo = new PersonRepo();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AmILoggedIn()
        {
            return new OkObjectResult(User.Identity.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Person person)
        {
            if (person.Username == null || person.Password == null)
            {
                return BadRequest();
            }

            Person attempt = null;

            // add a delay to the login call of 1.5 seconds to prevent timing attacks
            var delay = Task.Delay(1500);
            var login = Task.Run(() =>
            {
                attempt = repo.Auth(person);
            });
            await delay;

            // if the attempt was invalid
            if (attempt == null)
            {
                return new UnauthorizedResult();
            }

            // if the creds were correct setup our cookie/claims
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, attempt.Username),
                new Claim("Id", attempt.Id.ToString()),
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(14)
                }
            );

            return new OkResult();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new OkResult();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Person person)
        {
            if (person.Username == null || person.Password == null)
            {
                return BadRequest();
            }

            bool created = this.repo.Create(person);
            if (created)
            {
                return new OkResult();
            }

            return new ConflictResult();
        }
    }
}
