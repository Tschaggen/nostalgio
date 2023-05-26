using StdsSocialMediaBackend.Domain.Model.User;

namespace StdsSocialMediaBackend.Domain.Requests.Auth
{
    public class RegisterReq
    {
        public string Password { get; set; }
        public User User { get; set; }
    }
}
