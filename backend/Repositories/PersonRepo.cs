using System;
using backend.Models;
using Dapper;

namespace backend.Repositories
{
    public class PersonRepo : BaseRepo
    {
        public bool Create(Person person)
        {
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
            Person dbPerson = null;
            using (var connection = this.GetConn())
            {
                connection.Open();
                var query = "SELECT person_id as id, username, password FROM person WHERE username = @username;";
                dbPerson = connection.QuerySingle<Person>(query, person);
            }

            // check the supplied password matches what is saved in the db
            if (person.CheckPassword(dbPerson.Password))
            {
                return dbPerson;
            }

            // if hash doesn't match
            return null;
        }
    }
}