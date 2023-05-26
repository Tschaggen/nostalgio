namespace StdsSocialMediaBackend.Domain.Model.Auth
{
    public class AuthUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
