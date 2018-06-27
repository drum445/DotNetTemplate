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
        public async Task<IActionResult> Login([FromBody] Person Person)
        {
            Person attempt = repo.Auth(Person);

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
    }
}
