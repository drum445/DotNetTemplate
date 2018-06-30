using System;
using System.Collections.Generic;
using Dapper;
using backend.Models;

namespace backend.Repositories
{
    public class TodoRepo : BaseRepo
    {
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

        public List<Todo> Read(Guid personId)
        {
            var todos = new List<Todo>();
            using (var connection = this.GetConn())
            {
                connection.Open();
                var query = "SELECT todo_id AS id, title, body FROM todo WHERE person_id = @id";
                todos = connection.Query<Todo>(query, new { id = personId }).AsList();

            }
            return todos;
        }

        public Boolean Update(Todo todo)
        {
            using (var connection = this.GetConn())
            {
                connection.Open();
                var query = "UPDATE todo SET title = @title, body = @body WHERE todo_id = @Id AND person_id = @personId";
                connection.Execute(query, todo);
            }

            return true;
        }

        public Boolean Delete(string todoId, Guid personId)
        {
            using (var connection = this.GetConn())
            {
                connection.Open();
                var query = "DELETE FROM todo WHERE todo_id = @todoId AND person_id = @personId";
                connection.Execute(query, new { todoId = todoId, personId = personId });

            }
            return true;
        }
    }
}