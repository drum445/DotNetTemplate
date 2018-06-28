using System;
using backend.Models;
using MySql.Data.MySqlClient;

namespace backend.Repositories
{
    public class PersonRepo : BaseRepo
    {
        public bool Create(Person person)
        {
            if (this.Exists(person) == 1)
            {
                return false;
            }

            using (var connection = this.GetConn())
            {
                connection.Open();

                person.Id = Guid.NewGuid().ToString();
                person.HashPassword();

                var insertCmd = new MySqlCommand("INSERT INTO person VALUES(@id, @username, @password);", connection);
                insertCmd.Parameters.AddWithValue("id", person.Id);
                insertCmd.Parameters.AddWithValue("username", person.Username);
                insertCmd.Parameters.AddWithValue("password", person.Password);
                insertCmd.ExecuteNonQuery();
            }

            return true;
        }

        private Int32 Exists(Person person)
        {
            var count = 1;

            using (var connection = this.GetConn())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT COUNT(*) FROM person WHERE username = @username;", connection);
                command.Parameters.AddWithValue("username", person.Username);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                reader.Close();
            }

            return count;
        }

        public Person Auth(Person person)
        {
            // first check if the user exists
            if (this.Exists(person) == 0)
            {
                return null;
            }

            // load the person into memory 
            Person foundPerson = null;
            using (var connection = this.GetConn())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM person WHERE username = @username;", connection);
                command.Parameters.AddWithValue("username", person.Username);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var username = reader.GetString(1);
                    var password = reader.GetString(2);
                    foundPerson = new Person(username, password);
                    foundPerson.Id = reader["person_id"].ToString();
                }
                reader.Close();
            }

            // check the hash in the db matches the hash of the p/w supplied
            person.Id = foundPerson.Id;
            person.HashPassword();
            if (person.Password == foundPerson.Password)
            {
                return foundPerson;
            }

            // if hash doesn't match
            return null;

        }
    }
}