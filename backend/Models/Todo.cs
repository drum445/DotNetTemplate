using System;

namespace backend.Repositories
{
    public class Todo
    {
        public Todo() { }

        public Todo(Guid id, string title, string body, Guid personId)
        {
            this.Id = id;
            this.Title = title;
            this.Body = body;
            this.PersonId = personId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid? PersonId { get; set; }
    }
}