using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdsSocialMediaBackend.Domain.Requests.Media
{
    public class CommentReq
    {
        //public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
    }
}
