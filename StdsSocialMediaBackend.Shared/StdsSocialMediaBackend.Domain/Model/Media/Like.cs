namespace StdsSocialMediaBackend.Domain.Model.Media
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}
