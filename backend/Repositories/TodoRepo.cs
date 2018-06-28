using System.Collections.Generic;
using MySql.Data.MySqlClient;
using backend.Models;
using System;

namespace backend.Repositories
{
    public class TodoRepo : BaseRepo
    {
        public List<Todo> Get(string personId)
        {
            var todos = new List<Todo>();
            using (var connection = this.GetConn())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT todo_id, title, body FROM todo WHERE person_id = @personId;", connection);
                command.Parameters.AddWithValue("personId", personId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var title = reader["title"].ToString();
                        var body = reader["body"].ToString();
                        // null the personId as we want to omit from resp
                        var todo = new Todo(title, body, null);
                        todo.Id = reader["todo_id"].ToString();

                        todos.Add(todo);
                    }
                }
            }
            return todos;
        }

        public Todo Create(Todo todo)
        {
            todo.Id = Guid.NewGuid().ToString();

            using (var connection = this.GetConn())
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO todo VALUES(@id, @title, @body, @person);", connection);
                command.Parameters.AddWithValue("id", todo.Id);
                command.Parameters.AddWithValue("title", todo.Title);
                command.Parameters.AddWithValue("body", todo.Body);
                command.Parameters.AddWithValue("person", todo.PersonId);
                command.ExecuteNonQuery();
            }

            return todo;
        }
    }
}