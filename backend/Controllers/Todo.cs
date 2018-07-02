using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Repositories;
using backend.Common;
using backend.Models;
using System;

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

            return new OkObjectResult(todo);
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Read()
        {
            var personId = UserHelper.GetPersonId(User);
            var todos = repo.Read(personId);
            return new OkObjectResult(todos);
        }

        [HttpPut("{todoId}")]
        public IActionResult Update(Guid todoId, [FromBody] Todo todo)
        {
            if (todo.Title == null || todo.Body == null)
            {
                return BadRequest();
            }

            todo.Id = todoId;
            todo.PersonId = UserHelper.GetPersonId(User);
            repo.Update(todo);
            todo.PersonId = null;

            return new ObjectResult(todo);
        }

        [HttpDelete("{todoId}")]
        public ActionResult<IEnumerable<string>> Delete(Guid todoId)
        {
            var personId = UserHelper.GetPersonId(User);
            var todos = repo.Delete(todoId, personId);
            return new NoContentResult();
        }
    }
}