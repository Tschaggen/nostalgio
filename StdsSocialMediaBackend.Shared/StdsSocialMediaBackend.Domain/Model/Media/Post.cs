namespace StdsSocialMediaBackend.Domain.Model.Media
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public DateTime PostedAt { get; set; }
        public bool IsPublic { get; set; } = false;
        public string? Description { get; set; }
        public Guid OriginalImage { get; set; }
        public Guid? SmallImage { get; set; }
        public Guid? ThumbnailImage { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Like>? Likes { get; set; }

    }
}
