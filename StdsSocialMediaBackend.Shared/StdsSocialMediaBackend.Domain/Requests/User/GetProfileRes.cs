using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StdsSocialMediaBackend.Domain.Model.Media;
using StdsSocialMediaBackend.Domain.Model.User;

namespace StdsSocialMediaBackend.Domain.Requests.User
{
    public class GetProfileRes
    {
        public StdsSocialMediaBackend.Domain.Model.User.User User { get; set; }
        public List<Post>? Posts { get; set; }

    }
}
