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

            return new ObjectResult(todo);
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Read()
        {
            var personId = UserHelper.GetPersonId(User);
            var todos = repo.Read(personId);
            return new OkObjectResult(todos);
        }

        [HttpPut("{todoId}")]
        public IActionResult Update(string todoId, [FromBody] Todo todo)
        {
            if (todo.Title == null || todo.Body == null)
            {
                return BadRequest();
            }

            Guid newGuid;

            if(!Guid.TryParse(todoId, out newGuid))
            {
                return new BadRequestObjectResult("Invalid id format supplied");
            }

            todo.Id = newGuid;
            todo.PersonId = UserHelper.GetPersonId(User);
            repo.Update(todo);
            todo.PersonId = null;

            return new ObjectResult(todo);
        }

        [HttpDelete("{todoId}")]
        public ActionResult<IEnumerable<string>> Delete(string todoId)
        {
            // test for guid but don't use the result
            if(!Guid.TryParse(todoId, out var _))
            {
                return new BadRequestObjectResult("Invalid id format supplied");
            }

            var personId = UserHelper.GetPersonId(User);
            var todos = repo.Delete(todoId, personId);
            return new NoContentResult();
        }
    }
}