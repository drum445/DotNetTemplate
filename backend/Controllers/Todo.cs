using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Repositories;
using backend.Common;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TodoController : ControllerBase
    {
        protected TodoRepo repo;

        public TodoController()
        {
            this.repo = new TodoRepo();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Todo todo)
        {
            if (todo.Title == null || todo.Body == null)
            {
                return BadRequest();
            }

            todo.PersonId = UserHelper.GetPersonId(User);
            todo = repo.Create(todo);
            // null personId as we don't want it in resp
            todo.PersonId = null;

            return new ObjectResult(todo);
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Read()
        {
            var personId = UserHelper.GetPersonId(User);
            var todos = repo.Get(personId);
            return new OkObjectResult(todos);
        }
    }
}