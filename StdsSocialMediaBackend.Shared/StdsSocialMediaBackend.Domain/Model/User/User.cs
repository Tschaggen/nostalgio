namespace StdsSocialMediaBackend.Domain.Model.User
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Mobil { get; set; }
        public string? Biography { get; set; }
        public bool IsPublic { get; set; } = false;
        public Guid AdressId { get; set; }
        public Adress Adress { get; set; }
    }
}
