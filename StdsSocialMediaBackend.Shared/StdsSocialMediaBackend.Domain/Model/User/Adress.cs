namespace StdsSocialMediaBackend.Domain.Model.User
{
    public class Adress
    {
        public Guid Id { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Country { get; set; }
    }
}
