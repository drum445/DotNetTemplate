using System;

namespace backend.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid? PersonId { get; set; }
    }
}