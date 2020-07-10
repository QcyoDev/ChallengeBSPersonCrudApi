using PersonCrud.Api.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonCrud.Api.Models
{
    public class Person
    {
        public int PersonId { get; set; }

        public string DocType { get; set; }

        public string DocNum { get; set; }

        public string Country { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public List<Contact> ContactDetails { get; set; }

        [InverseProperty("Person")]
        public List<Relationship> Relationships { get; set; }
    }
}
