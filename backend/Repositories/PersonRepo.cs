using backend.Models;

namespace backend.Repositories
{
    public class PersonRepo : BaseRepo
    {
        public Person Auth(Person person)
        {
            if (person.Username == "drum")
            {
                person.Id = "1";
                return person;
            }
            else
            {
                return null;
            }
        }
    }
}