namespace backend.Repositories
{
    public class Todo
    {
        public Todo(string title, string body, string personId)
        {
            this.Title = title;
            this.Body = body;
            this.PersonId = personId;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string PersonId { get; set; }
    }
}