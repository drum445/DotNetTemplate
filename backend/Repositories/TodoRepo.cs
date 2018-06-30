using System;
using System.Collections.Generic;
using Dapper;
using backend.Models;

namespace backend.Repositories
{
    public class TodoRepo : BaseRepo
    {
        public List<Todo> Get(Guid personId)
        {
            var todos = new List<Todo>();
            using (var connection = this.GetConn())
            {
                connection.Open();
                var query = "select todo_id as id, title, body from todo where person_id = @id";
                todos = connection.Query<Todo>(query, new { id = personId }).AsList();

            }
            return todos;
        }

        public Todo Create(Todo todo)
        {
            todo.Id = Guid.NewGuid();

            using (var connection = this.GetConn())
            {
                connection.Open();
                var query = "INSERT INTO todo VALUES(@id, @title, @body, @personId);";
                connection.Execute(query, todo);
            }

            return todo;
        }
    }
}