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
        protected UserHelper helper;

        public TodoController()
        {
            this.repo = new TodoRepo();
            this.helper = new UserHelper();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Todo todo)
        {
            if (todo.Title == null || todo.Body == null)
            {
                return BadRequest();
            }

            todo.PersonId = helper.GetPersonId(User);
            todo = repo.Create(todo);
            // null personId as we don't want it in resp
            todo.PersonId = null;

            return new ObjectResult(todo);
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Read()
        {
            var personId = helper.GetPersonId(User);
            var todos = repo.Get(personId);
            return new OkObjectResult(todos);
        }
    }
}