namespace PersonCrud.Api.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public ContactType ContactType { get; set; }
        public string ContactInfo { get; set; }

        public Person Person { get; set; }

    }
}