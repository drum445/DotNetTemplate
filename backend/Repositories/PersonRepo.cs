using System;
using backend.Models;
using MySql.Data.MySqlClient;
using Dapper;

namespace backend.Repositories
{
    public class PersonRepo : BaseRepo
    {
        public bool Create(Person person)
        {
            // perform the hash first to prevent timing attacks
            person.Id = Guid.NewGuid();
            person.HashPassword();

            if (this.Exists(person) == 1)
            {
                return false;
            }

            using (var connection = this.GetConn())
            {
                connection.Open();
                var query = "INSERT INTO person VALUES(@id, @username, @password);";
                connection.Execute(query, person);
            }

            return true;
        }

        private Int32 Exists(Person person)
        {
            var count = 1;
            using (var connection = this.GetConn())
            {
                connection.Open();
                var query = "SELECT COUNT(person_id) FROM person WHERE username = @username;";
                count = connection.ExecuteScalar<Int32>(query, person);
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
                var query = "SELECT person_id as id, username, password FROM person WHERE username = @username;";
                foundPerson = connection.QuerySingle<Person>(query, person);
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