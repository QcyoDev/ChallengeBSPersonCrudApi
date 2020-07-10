using PersonCrud.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonCrud.Api.Dtos
{
    public class PersonGetDto
    {
        public int PersonId { get; set; }

        public string DocType { get; set; }

        public string DocNum { get; set; }

        public string Country { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public List<ContactGetDto> ContactDetails { get; set; }
    }
}
