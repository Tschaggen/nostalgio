namespace StdsSocialMediaBackend.Domain.Requests.Auth
{
    public class AuthRes
    {
        public string JwtToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
