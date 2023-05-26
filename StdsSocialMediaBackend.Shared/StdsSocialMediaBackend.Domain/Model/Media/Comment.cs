namespace StdsSocialMediaBackend.Domain.Model.Media
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }
    }
}
