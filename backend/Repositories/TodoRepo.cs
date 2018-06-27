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
                var command = new MySqlCommand("SELECT 'a' as 'title', 'b' as 'body' FROM DUAL;", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var title = reader["title"].ToString();
                        var body = reader["body"].ToString();
                        var todo = new Todo(title, body, personId);
                        todo.Id = "1234";
                        
                        todos.Add(todo);
                    }
                }
            }
            return todos;
        }

        public Todo Create(Todo todo)
        {
            todo.Id = Guid.NewGuid().ToString();
            return todo;
        }
    }
}