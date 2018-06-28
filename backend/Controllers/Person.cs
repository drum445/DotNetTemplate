using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Repositories;
using backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        protected PersonRepo repo;

        public PersonController()
        {
            repo = new PersonRepo();
        }

        [HttpGet]
        public IActionResult AmILoggedIn()
        {
            if (User.Identity.Name == null)
            {
                return new UnauthorizedResult();
            }
            return new OkObjectResult(User.Identity.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Person person)
        {
            if (person.Username == null || person.Password == null)
            {
                return BadRequest();
            }

            Person attempt = repo.Auth(person);
            if (attempt == null)
            {
                return new UnauthorizedResult();
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, attempt.Username),
                new Claim("Id", attempt.Id),
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
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
            else
            {
                return new ConflictResult();
            }
        }
    }
}
