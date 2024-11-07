namespace Talabat.Core.Entities.OrderAggregate
{
    public class Address
    {
        // Accessable empty parameterless ctor for EF Core migration
        public Address() { }
        // Parameter ctor for my usage
        public Address(string fName, string lName, string country, string city, string street)
        {
            FirstName = fName;
            LastName = lName;
            Country = country;
            City = city;
            Street = street;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
