using Microsoft.AspNetCore.Mvc;
using StdsSocialMediaBackend.Domain.Model.Media;

namespace StdsSocialMediaBackend.Domain.Requests.Media
{
    public class GetPostRes
    {
        public Guid PostId { get; set; }
        public DateTime PostetAt { get; set; }
        public Guid PostetByUserId { get; set; }
        public string PostetByUsername { get; set; }
        public string? Description { get; set; }
        public int Likes { get; set; }
        public List<Comment>? Comments { get; set; }
        public FileContentResult? Image { get; set; }
    }
}
