namespace Talabat.Core.Entities.OrderAggregate
{
    public class Address
    {
        // Accessable empty parameterless ctor for EF Core migration
        public Address() { }
        // Parameter ctor for my usage
        public Address(string fName, string lName, string country, string city, string street)
        {
            FName = fName;
            LName = lName;
            Country = country;
            City = city;
            Street = street;
        }

        public string FName { get; set; }
        public string LName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
